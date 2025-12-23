using System;
using System.Xml;
using System.IO;
using System.Linq;

namespace FileMonitor
{
    public class LiveBCast
    {
        // Поля для хранения конфигурации
        private string _searchKeyword = "ГЗ - прямой эфир";
        private string _loggerFolder;
        private string _copyFolder;

        // Конструктор
        public LiveBCast(string loggerFolder, string copyFolder)
        {
            _loggerFolder = loggerFolder;
            _copyFolder = copyFolder;
        }

        // Метод для запуска мониторинга
        public void StartMonitoring()
        {
            while (true)
            {
                // Поиск файлов за последний час с проверкой шаблона и времени
                var files = Directory.GetFiles(_loggerFolder, "full_playing_*.xml")
                    .Where(file =>
                        File.GetLastWriteTime(file) <= DateTime.UtcNow.AddHours(-1) &&
                        System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileName(file), @"^full_playing_\d{8}_\d{6}\.xml$")
                    ).ToArray();

                foreach (var file in files)
                {
                    ProcessFile(file);
                }

                // Ожидание нового файла
                System.Threading.Thread.Sleep(600000);
            }
        }

        // Обработка одного файла
        private void ProcessFile(string filePath)
        {
            try
            {
                // Чтение файла как текста
                var lines = File.ReadAllLines(filePath);
                bool foundFirst = false;
                string currentName = null;
                DateTime? currentStartTime = null;

                // Обработка строк файла
                for (int i = 0; i < lines.Length; i++)
                {
                    // Проверка на начало блока
                    if (lines[i].StartsWith(@"<?xml version=""1.0"" encoding=""utf-8"" ?>") && !foundFirst)
                    {
                        // Парсинг блока
                        var block = new XmlDocument();
                        block.LoadXml(lines[i]);
                        var nameNode = block.SelectSingleNode("//NAME");
                        var startNode = block.SelectSingleNode("//START_TIME");

                        if (nameNode?.InnerText == _searchKeyword && nameNode != null && startNode != null)
                        {
                            currentName = nameNode.InnerText;
                            currentStartTime = DateTime.ParseExact(startNode.InnerText, "yyyyMMdd_HHmmss", null);
                            foundFirst = true;
                        }
                    }

                    // Если блок найден, ищем MP3-файл
                    if (foundFirst && currentStartTime.HasValue)
                    {
                        var roundedStartTime = new DateTime(currentStartTime.Value.Year, currentStartTime.Value.Month, currentStartTime.Value.Day, currentStartTime.Value.Hour, currentStartTime.Value.Minute, 0);
                        var searchTime = roundedStartTime.AddMinutes(-1);
                        while (searchTime <= roundedStartTime.AddMinutes(1))
                        {
                            var mp3File = Path.Combine(_copyFolder, $"{searchTime:HH-mm-ss}.mp3");
                            if (File.Exists(mp3File))
                            {
                                var newFileName = Path.Combine(_copyFolder, currentName + ".mp3");
                                File.Move(mp3File, newFileName);
                                UpdateDashboard($"Найден файл: {Path.GetFileName(mp3File)} -> {Path.GetFileName(newFileName)");
                                break;
                            }

                            searchTime = searchTime.AddMinutes(1);
                        }

                        // Если файл не найден
                        if (searchTime > roundedTime.AddMinutes(1))
                        {
                            UpdateDashboard("Файл эфира не найден в указанном интервале");
                        }

                        // Удаление обработанного файла
                        File.Delete(filePath);
                    }
                }

                    // Округление времени до минут
                    var roundedStartTime = new DateTime(currentStartTime.Value.Year, currentStartTime.Value.Month, currentStartTime.Value.Day, currentStartTime.Value.Hour, currentStartTime.Value.Minute, 0);

                    // Поиск MP3 файла в 3-минутном интервале
                    var searchTimeInner = roundedTime.AddMinutes(-1);
                    while (searchTime <= roundedStartTime.AddMinutes(1))
                    {
                        var mp3File = Path.Combine(_copyFolder, $"{searchTime:HH-mm-ss}.mp3");
                        if (File.Exists(mp3File))
                        {
                            // Переименование файла
                            var newFileName = Path.Combine(_copyFolder, lastNode.SelectSingleNode("NAME").InnerText + ".mp3");
                            File.Move(mp3File, newFileName);

                            // Обновление дашборда
                            UpdateDashboard($"Найден файл: {Path.GetFileName(mp3File)} -> {Path.GetFileName(newFileName)}");
                            break;
                        }

                        searchTime = searchTime.AddMinutes(1);
                    }

                    // Если файл не найден
                    if (searchTime > roundedTime.AddMinutes(1))
                    {
                        UpdateDashboard("Файл эфира не найден в указанном интервале");
                    }

                    // Удаление обработанного файла
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                UpdateDashboard($"Ошибка обработки файла: {ex.Message}");
            }
        }

        // Метод для обновления дашборда
        private void UpdateDashboard(string message)
        {
            // Реализация вывода сообщения в дашборд
            Console.WriteLine(message);
        }
    }
}
