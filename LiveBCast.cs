using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace FileMonitor
{
    public class LiveBCast
    {
        // Поля для хранения конфигурации
        private string _searchKeyword;
        private string _loggerFolder;
        private string _copyFolder;
        private bool doit =true;
        RichTextBox _richTextBox;

        // Конструктор
        public LiveBCast(string loggerFolder, string copyFolder, RichTextBox richTextBox, string searchKeyword = "ГЗ - прямой эфир")
        {
            _loggerFolder = loggerFolder;
            _copyFolder = copyFolder;
            _searchKeyword = searchKeyword;
            _richTextBox = richTextBox;
            doit = true;
        }

        // Метод для запуска мониторинга
        public void StopMonitoring()
        {
            doit = false;
        }
        public async void StartMonitoring()
        {
            while (doit)
            {
                // Поиск файлов за последний час с проверкой шаблона и времени
                var files = Directory.GetFiles(_loggerFolder, "full_playing_*.xml")
                    .Where(file =>
                        File.GetLastWriteTime(file) >= DateTime.UtcNow.AddHours(-1) &&
                        System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileName(file), @"^full_playing_\d{8}_\d{6}\.xml$")
                    ).ToArray();

                foreach (var file in files)
                {
                    ProcessFile(file);
                }

                // Асинхронное ожидание нового файла
                await Task.Delay(600000);
            }
        }

        // Обработка одного файла
        private void ProcessFile(string filePath)
        {
            try
            {
                // Чтение файла как текста
                var content = File.ReadAllText(filePath);

                // Разделение содержимого на блоки по комментариям
                var blocks = content.Split(new string[] { "<!--" }, StringSplitOptions.RemoveEmptyEntries);

                // Пропускаем первый элемент, так как он до первого комментария
                for (int i = 1; i < blocks.Length; i++)
                {
                    var blockContent = "<!--" + blocks[i]; // Восстанавливаем начальный комментарий

                    // Находим конец текущего блока (до следующего комментария или до конца)
                    var nextCommentIndex = blockContent.IndexOf("<!--", 1);
                    if (nextCommentIndex > 0)
                    {
                        var actualBlock = blockContent.Substring(0, nextCommentIndex);
                        ProcessBlock(actualBlock);
                    }
                    else
                    {
                        ProcessBlock(blockContent);
                    }
                }

                // Удаление обработанного файла
              //  File.Delete(filePath);
            }
            catch (Exception ex)
            {
                UpdateDashboard($"Ошибка обработки файла: {ex.Message}");
            }
        }

        // Обработка одного XML-блока
        private void ProcessBlock(string blockContent)
        {
            try
            {
                // Удаление начального комментария перед парсингом XML
                var cleanContent = blockContent;
                if (cleanContent.StartsWith("<!--"))
                {
                    var endCommentIndex = cleanContent.IndexOf("-->");
                    if (endCommentIndex > 0)
                    {
                        cleanContent = cleanContent.Substring(endCommentIndex + 3).Trim();
                    }
                }

                // Парсинг блока как XML
                var block = new XmlDocument();
                block.LoadXml(cleanContent);

                // Поиск всех элементов с тегом NAME
                var nameNodes = block.SelectNodes("//NAME");
                var startTimeNodes = block.SelectNodes("//START_TIME");

                // Ищем последовательные блоки с ключевой фразой
                XmlNode lastNameNode = null;
                XmlNode lastStartTimeNode = null;
                XmlNode firstNameNode = null;
                XmlNode firstStartTimeNode = null;
                bool foundSequence = false;

                for (int i = 0; i < nameNodes.Count; i++)
                {
                    if (nameNodes[i].InnerText.Contains(_searchKeyword))
                    {
                        if (!foundSequence)
                        {
                            // Это начало последовательности
                            firstNameNode = nameNodes[i];
                            firstStartTimeNode = startTimeNodes[i];
                            foundSequence = true;
                        }
                        // Запоминаем последний элемент последовательности
                        lastNameNode = nameNodes[i];
                        lastStartTimeNode = startTimeNodes[i];
                    }
                    else if (foundSequence)
                    {
                        // Последовательность закончилась, выходим из цикла
                        break;
                    }
                }

                if (foundSequence && firstNameNode != null && firstStartTimeNode != null)
                {
                    // Используем время из первого элемента последовательности
                    var startTimeText = firstStartTimeNode.InnerText;
                    DateTime startTime;

                    // Обработка времени в форматах "HH:mm:ss" или "yyyyMMdd_HHmmss"
                    if (startTimeText.Contains(":"))
                    {
                        var datePart = block.SelectSingleNode("//START_DATE")?.InnerText ?? DateTime.Now.ToString("yyyy-MM-dd");
                        startTime = DateTime.Parse($"{datePart} {startTimeText}");
                    }
                    else
                    {
                        startTime = DateTime.ParseExact(startTimeText, "yyyyMMdd_HHmmss", null);
                    }

                    // Округление времени до минут
                    var roundedStartTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, 0);

                    // Поиск MP3 файла в 3-минутном интервале
                    var searchTime = roundedStartTime.AddMinutes(-1);
                    bool fileFound = false;
                    while (searchTime <= roundedStartTime.AddMinutes(1))
                    {
                        // Поиск всех файлов вида HH-mm-*.mp3 в указанной папке
                        var pattern = $"{searchTime:HH-mm}-*.mp3";
                        var files = Directory.GetFiles(_copyFolder, pattern);

                        if (files.Length > 0)
                        {
                            // Берем первый найденный файл
                            var mp3File = files[0];

                            // Переименование файла
                            var newFileName = Path.Combine(_copyFolder, lastNameNode.InnerText + ".mp3");
                            File.Move(mp3File, newFileName);

                            // Обновление дашборда
                            UpdateDashboard($"Найден файл: {Path.GetFileName(mp3File)} -> {Path.GetFileName(newFileName)}");
                            fileFound = true;
                            break;
                        }

                        searchTime = searchTime.AddMinutes(1);
                    }

                    // Если файл не найден
                    if (!fileFound)
                    {
                        UpdateDashboard("Файл эфира не найден в указанном интервале");
                    }
                }
            }
            catch (XmlException)
            {
                // Игнорируем ошибки парсинга XML, так как блок может быть неполным
            }
            catch (Exception ex)
            {
                UpdateDashboard($"Ошибка обработки блока: {ex.Message}");
            }
        }

        // Метод для обновления дашборда
        private void UpdateDashboard(string message)
        {
            // Реализация вывода сообщения в дашборд
            _richTextBox.AppendText(message);
        }
    }
}
