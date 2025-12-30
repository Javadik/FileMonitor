using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace FileMonitor
{
    public class LiveBCast
    {
        // Класс для хранения информации о найденном вхождении
        public class FoundOccurrence
        {
            public string FileName { get; set; }
            public DateTime StartTime { get; set; }
            public string Name { get; set; }
            public bool Processed { get; set; }

            public FoundOccurrence(string fileName, DateTime startTime, string name, bool processed)
            {
                FileName = fileName;
                StartTime = startTime;
                Name = name;
                Processed = processed;
            }
        }

        // Поля для хранения конфигурации
        private string _searchKeyword;
        private string _loggerFolder;
        private string _copyFolder;
        private bool doit =true;
        RichTextBox _richTextBox;
        private List<FoundOccurrence> _foundOccurrences = new List<FoundOccurrence>();
        private HashSet<string> _processedFiles = new HashSet<string>();

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
                        File.GetLastWriteTime(file) <= DateTime.UtcNow.AddHours(-1) &&
                        System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileName(file), @"^full_playing_\d{8}_\d{6}\.xml$")
                    ).ToArray();

                foreach (var file in files)
                {
                    // Проверяем, был ли файл уже обработан
                    if (!_processedFiles.Contains(Path.GetFileName(file)))
                    {
                        ProcessFile(file);
                        // Добавляем файл в список обработанных
                        _processedFiles.Add(Path.GetFileName(file));
                    }
                }

                // Асинхронное ожидание нового файла
                await Task.Delay(60000); //600000 -10min
            }
        }

        // Обработка одного файла
        private void ProcessFile(string filePath)
        {
            try
            {
                UpdateDashboard($"Начата обработка файла: {filePath}");

                // Чтение файла как текста
                var content = File.ReadAllText(filePath);
                //UpdateDashboard($"Размер файла: {content.Length} символов"); // KCode edit

                // Разделение содержимого на блоки по комментариям
                var blocks = content.Split(new string[] { "<!--" }, StringSplitOptions.RemoveEmptyEntries);
                UpdateDashboard($"Найдено блоков: {blocks.Length}"); // KCode edit

                // Пропускаем первый элемент, так как он до первого комментария
                for (int i = 1; i < blocks.Length; i++)
                {
                    var blockContent = "<!--" + blocks[i]; // Восстанавливаем начальный комментарий

                    // Находим конец текущего блока (до следующего комментария или до конца)
                    var nextCommentIndex = blockContent.IndexOf("<!--", 1);
                    if (nextCommentIndex > 0)
                    {
                        var actualBlock = blockContent.Substring(0, nextCommentIndex);
                        if (!ProcessBlock(actualBlock, filePath)) // Если блок содержит вхождение, пропускаем последующие блоки с вхождениями
                        {
                            // Пропускаем все последующие блоки, которые содержат вхождения ключевого слова
                            i = SkipConsecutiveKeywordBlocks(blocks, i, nextCommentIndex, filePath);
                        }
                    }
                    else
                    {
                        ProcessBlock(blockContent, filePath);
                    }
                }

                // Удаление обработанного файла
              // File.Delete(filePath);
               // UpdateDashboard($"Обработка файла завершена: {Path.GetFileName(filePath)}"); // KCode edit
            }
            catch (UnauthorizedAccessException ex)
            {
                UpdateDashboard($"Ошибка доступа к файлу {Path.GetFileName(filePath)} в методе ProcessFile: {ex.Message}"); // KCode edit
            }
            catch (IOException ex)
            {
                UpdateDashboard($"Ошибка ввода-вывода при обработке файла {Path.GetFileName(filePath)} в методе ProcessFile: {ex.Message}"); // KCode edit
            }
            catch (Exception ex)
            {
                UpdateDashboard($"Неизвестная ошибка обработки файла {Path.GetFileName(filePath)} в методе ProcessFile: {ex.Message}"); // KCode edit
                //UpdateDashboard($"Тип ошибки: {ex.GetType().Name}"); // KCode edit
                //UpdateDashboard($"Стек вызова: {ex.StackTrace}"); // KCode edit
            }
        }

        // Обработка одного XML-блока, возвращает true если блок не содержит ключевого слова
        private bool ProcessBlock(string blockContent, string fileName)
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

                // Проверяем, есть ли в блоке вхождения ключевого слова
                bool hasKeyword = false;
                XmlNode? firstNameNode = null;
                XmlNode? firstStartTimeNode = null;

                for (int i = 0; i < nameNodes?.Count; i++)
                {
                    if (nameNodes[i]?.InnerText?.Contains(_searchKeyword) == true)
                    {
                        if (!hasKeyword)
                        {
                            // Это первое вхождение в блоке
                            firstNameNode = nameNodes[i];
                            firstStartTimeNode = startTimeNodes?[i];
                            hasKeyword = true;
                        }
                    }
                }

                if (hasKeyword && firstNameNode?.InnerText != null && firstStartTimeNode?.InnerText != null)
                {
                    // Используем время из первого элемента с вхождением
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

                    // Проверяем, есть ли уже запись с похожим временем именем
                    if (!IsDuplicateEntry(Path.GetFileName(fileName), roundedStartTime, firstNameNode.InnerText))
                    {
                        // Добавляем запись в список найденных вхождений
                        _foundOccurrences.Add(new FoundOccurrence(Path.GetFileName(fileName), roundedStartTime, firstNameNode.InnerText, false));

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
                                var newFileName = Path.Combine(_copyFolder, firstNameNode.InnerText + ".mp3");
                                File.Move(mp3File, newFileName);

                                // Обновляем статус обработки в списке
                                var occurrence = _foundOccurrences.Last();
                                occurrence.Processed = true;

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

                // Возвращаем true, если блок не содержал ключевое слово
                return !hasKeyword;
            }
            catch (XmlException)
            {
                // Игнорируем ошибки парсинга XML, так как блок может быть неполным
                return true; // Возвращаем true, чтобы продолжить обработку
            }
            catch (Exception ex)
            {
                UpdateDashboard($"Ошибка обработки блока: {ex.Message}");
                return true; // Возвращаем true, чтобы продолжить обработку
            }
        }

        // Метод для обновления дашборда
        private void UpdateDashboard(string message)
        {
            // Реализация вывода сообщения в дашборд
            _richTextBox.AppendText(message + Environment.NewLine);
        }

        // Метод для пропуска последовательных блоков с вхождениями ключевого слова
        private int SkipConsecutiveKeywordBlocks(string[] blocks, int currentIndex, int nextCommentIndex, string fileName)
        {
            int i = currentIndex;
            // Пропускаем текущий блок, так как он уже был обработан
            i++;

            // Продолжаем пропускать блоки, пока они содержат вхождения ключевого слова
            while (i < blocks.Length)
            {
                var nextBlockContent = "<!--" + blocks[i]; // Восстанавливаем начальный комментарий

                // Находим конец текущего блока (до следующего комментария или до конца)
                var nextNextCommentIndex = nextBlockContent.IndexOf("<!--", 1);
                var actualNextBlock = nextBlockContent;
                if (nextNextCommentIndex > 0)
                {
                    actualNextBlock = nextBlockContent.Substring(0, nextNextCommentIndex);
                }

                // Проверяем, содержит ли следующий блок ключевое слово
                if (!BlockContainsKeyword(actualNextBlock, fileName))
                {
                    // Если блок не содержит ключевое слово, останавливаем пропуск
                    break;
                }

                i++;
            }

            return i - 1; // Возвращаем индекс, с которого нужно продолжить обработку
        }

        // Вспомогательный метод для проверки, содержит ли блок ключевое слово
        private bool BlockContainsKeyword(string blockContent, string fileName)
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

                // Проверяем, есть ли в блоке вхождения ключевого слова
                for (int i = 0; i < nameNodes?.Count; i++)
                {
                    if (nameNodes[i]?.InnerText?.Contains(_searchKeyword) == true)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (XmlException)
            {
                // Игнорируем ошибки парсинга XML, так как блок может быть неполным
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Метод для проверки дубликата в списке найденных вхождений
        private bool IsDuplicateEntry(string fileName, DateTime startTime, string name)
        {
            // Проверяем, есть ли уже запись с похожим временем (разница не более 2 минут) и с тем же именем
            return _foundOccurrences.Any(occ =>
                Math.Abs((occ.StartTime - startTime).TotalMinutes) <= 2 &&
                occ.Name == name);
        }
    }
}
