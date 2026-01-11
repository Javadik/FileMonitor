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
            public string StartDate { get; set; }
            public bool Processed { get; set; }

            public FoundOccurrence(string fileName, DateTime startTime, string name, string startDate, bool processed)
            {
                FileName = fileName;
                StartTime = startTime;
                Name = name;
                StartDate = startDate;
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
        public LiveBCast(string loggerFolder, string copyFolder, RichTextBox richTextBox,
            string searchKeyword = "прямой эфир")
        {
            _loggerFolder = loggerFolder;
            _copyFolder = copyFolder;
            _searchKeyword = searchKeyword;
            _richTextBox = richTextBox;
            doit = true;

            // Загрузка сохраненных данных при инициализации
            LoadSavedData();
        }

        // Метод для сохранения данных
        public void SaveData()
        {
            try
            {
                // Сохраняем _foundOccurrences вручную
                var occurrencesLines = new List<string>();
                foreach (var occurrence in _foundOccurrences)
                {
                    occurrencesLines.Add($"{occurrence.FileName}|{occurrence.StartTime:yyyy-MM-dd HH:mm:ss}|{occurrence.Name}|{occurrence.StartDate}|{occurrence.Processed}");
                }
                File.WriteAllLines("found_occurrences.txt", occurrencesLines);

                // Сохраняем _processedFiles
                File.WriteAllLines("processed_files.txt", _processedFiles);

                UpdateDashboard("Данные успешно сохранены");
            }
            catch (Exception ex)
            {
                UpdateDashboard($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        // Метод для загрузки сохраненных данных
        private void LoadSavedData()
        {
            try
            {
                // Загружаем _foundOccurrences
                if (File.Exists("found_occurrences.txt"))
                {
                    var lines = File.ReadAllLines("found_occurrences.txt");
                    _foundOccurrences = new List<FoundOccurrence>();

                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 5)
                        {
                            if (DateTime.TryParse(parts[1], out DateTime startTime) &&
                                bool.TryParse(parts[4], out bool processed))
                            {
                                _foundOccurrences.Add(new FoundOccurrence(parts[0], startTime, parts[2], parts[3], processed));
                            }
                        }
                    }

                    UpdateDashboard($"Загружено {_foundOccurrences.Count} найденных вхождений");
                }

                // Загружаем _processedFiles
                if (File.Exists("processed_files.txt"))
                {
                    var fileLines = File.ReadAllLines("processed_files.txt");
                    _processedFiles = new HashSet<string>(fileLines);
                    UpdateDashboard($"Загружено {_processedFiles.Count} обработанных файлов");
                }
            }
            catch (Exception ex)
            {
                UpdateDashboard($"Ошибка при загрузке данных: {ex.Message}");
                // Инициализируем пустыми коллекциями в случае ошибки
                _foundOccurrences = new List<FoundOccurrence>();
                _processedFiles = new HashSet<string>();
            }
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
                        File.GetLastWriteTime(file) <= DateTime.UtcNow.AddMinutes(-1) &&  //AddHours(-1) AddMinutes
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
                var blocks = content.Split(new string[] { "<!--" }, StringSplitOptions.None);
                UpdateDashboard($"Найдено блоков: {blocks.Length}"); // KCode edit

                // Обрабатываем все элементы массива
                for (int i = 0; i < blocks.Length; i++)
                {
                    // Пропускаем первый элемент, если он пустой (до первого комментария)
                    if (i == 0 && string.IsNullOrEmpty(blocks[i]))
                    {
                        continue;
                    }

                    var blockContent = "<!--" + blocks[i]; // Восстанавливаем начальный комментарий

                    // Проверяем, содержит ли блок ключевое слово
                    if (BlockContainsKeyword(blockContent, Path.GetFileName(filePath)))
                    {


                        // Извлекаем содержимое комментария для отображения
                        string commentContent = ExtractCommentContent(blocks[i]);
                        UpdateDashboard($"Обработка блока с вхождением <!--{commentContent}");

                        // Обрабатываем блок с вхождением
                        ProcessBlock(blockContent, filePath);

                        // Пропускаем все последовательные блоки с вхождениями
                        int j = i + 1;
                        while (j < blocks.Length)
                        {
                            // Извлекаем содержимое комментария для отображения
                            string nextCommentContent = ExtractCommentContent(blocks[j]);

                            var nextBlockContent = "<!--" + blocks[j];
                            if (BlockContainsKeyword(nextBlockContent, Path.GetFileName(filePath)))
                            {
                                // Обрабатываем пропускаемый блок, но не выводим сообщение о пропуске
                                //--ProcessBlock(nextBlockContent, filePath);
                                UpdateDashboard($"Пропускаем блок с вхождением <!--{nextCommentContent}");//--: {Path.GetFileName(filePath)}");
                                j++;
                            }
                            else
                            {
                                UpdateDashboard($"Найден блок без вхождения <!--{nextCommentContent}, продолжаем обработку");
                                break; // Нашли блок без вхождения, выходим из цикла пропуска
                            }
                        }

                        // Обновляем i, чтобы пропустить обработанные блоки
                        if (j > i + 1) {
                            i = j - 1; // Устанавливаем i на последний пропущенный блок
                        }
                    }
                    /*--else
                    {
                        // Обрабатываем обычный блок
                        ProcessBlock(blockContent, filePath);
                    }
                    */
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

        // Обработка одного XML-блока
        private void ProcessBlock(string blockContent, string fileName)
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

                // Поиск всех элементов ELEM
                var elemNodes = block.SelectNodes("//ELEM");

                // Проверяем, есть ли в блоке вхождения ключевого слова
                bool hasKeyword = false;
                XmlNode? foundNameNode = null;
                XmlNode? foundStartTimeNode = null;

                for (int i = 0; i < elemNodes.Count; i++)
                {
                    XmlNode elem = elemNodes[i];
                    var nameNode = elem.SelectSingleNode("NAME");
                    var startTimeNode = elem.SelectSingleNode("START_TIME");

                    if (nameNode?.InnerText?.Contains(_searchKeyword) == true)
                    {
                        // Это элемент с ключевым словом
                        foundNameNode = nameNode;

                        // Если в этом элементе нет START_TIME, ищем в предыдущих элементах
                        if (startTimeNode == null)
                        {
                            // Ищем START_TIME в предыдущих элементах
                            for (int j = i - 1; j >= 0; j--)
                            {
                                var prevElem = elemNodes[j];
                                var prevStartTimeNode = prevElem.SelectSingleNode("START_TIME");
                                if (prevStartTimeNode != null && prevStartTimeNode.InnerText != null)
                                {
                                    foundStartTimeNode = prevStartTimeNode;
                                    break;
                                }
                            }

                            // Если не нашли в предыдущих, ищем в следующих
                            if (foundStartTimeNode == null)
                            {
                                for (int j = i + 1; j < elemNodes.Count; j++)
                                {
                                    var nextElem = elemNodes[j];
                                    var nextStartTimeNode = nextElem.SelectSingleNode("START_TIME");
                                    if (nextStartTimeNode != null && nextStartTimeNode.InnerText != null)
                                    {
                                        foundStartTimeNode = nextStartTimeNode;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // START_TIME найден в текущем элементе
                            foundStartTimeNode = startTimeNode;
                        }

                        hasKeyword = true;
                        break; // Берем первый элемент, содержащий ключевое слово
                    }
                }

                if (hasKeyword && foundNameNode?.InnerText != null && foundStartTimeNode?.InnerText != null)
                {
                    // Используем время из элемента с вхождением
                    var startTimeText = foundStartTimeNode.InnerText;
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

                    // Получаем дату из START_DATE
                    var startDate = block.SelectSingleNode("//START_DATE")?.InnerText ?? DateTime.Now.ToString("yyyy-MM-dd");

                    // Проверяем, есть ли уже запись с похожим временем именем в _foundOccurrences
                    if (!IsDuplicateEntry(Path.GetFileName(fileName), roundedStartTime, foundNameNode.InnerText))
                    {
                        UpdateDashboard($"Найдено вхождение: {foundNameNode.InnerText}, время: {roundedStartTime}");

                        // Добавляем запись в список найденных вхождений
                        _foundOccurrences.Add(new FoundOccurrence(Path.GetFileName(fileName), roundedStartTime, foundNameNode.InnerText, startDate, false));

                        // Поиск MP3 файла в 3-минутном интервале
                        var searchTime = roundedStartTime.AddMinutes(-1);
                        bool fileFound = false;
                        UpdateDashboard($"Поиск файла для: {foundNameNode.InnerText}, время: {roundedStartTime}, интервал: [{searchTime:HH-mm} - {roundedStartTime.AddMinutes(1):HH-mm}]");

                        // Получаем дату из roundedStartTime для построения пути
                        var datePart = roundedStartTime.ToString("yyyy-MM-dd");
                        var dateFolder = Path.Combine(_copyFolder, datePart);

                        while (searchTime <= roundedStartTime.AddMinutes(1))
                        {
                            // Поиск всех файлов вида HH-mm-*.mp3 в папке даты
                            var pattern = $"{searchTime:HH-mm}-*.mp3";
                            var files = Directory.GetFiles(dateFolder, pattern);
                            UpdateDashboard($"Поиск по паттерну: {pattern} в папке {dateFolder}, найдено файлов: {files.Length}");

                            if (files.Length > 0)
                            {
                                // Берем первый найденный файл
                                var mp3File = files[0];
                                UpdateDashboard($"Найден файл: {mp3File}");

                                // Переименование файла с проверкой на существование
                                var newFileName = GetUniqueFileName(Path.Combine(dateFolder, foundNameNode.InnerText + ".mp3"));
                                File.Move(mp3File, newFileName);

                                // Обновляем статус обработки в списке
                                var occurrence = _foundOccurrences.Last();
                                occurrence.Processed = true;

                                // Обновление дашборда
                                UpdateDashboard($"Файл переименован: {Path.GetFileName(mp3File)} -> {Path.GetFileName(newFileName)}");
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
                    else
                    {
                        UpdateDashboard($"Пропущено дублирующееся вхождение: {foundNameNode.InnerText}, время: {roundedStartTime}");
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
            // Реализация вывода сообщения в дашборд с префиксом
            _richTextBox.AppendText("<<lv<" + message + Environment.NewLine);
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
                var doc = new XmlDocument();
                doc.LoadXml(cleanContent);

                // Рекурсивный обход всех узлов для поиска элементов NAME
                string blockId = ExtractCommentContent(blockContent); // Извлекаем идентификатор блока для отладки
                return SearchForKeywordInNodes(doc, _searchKeyword, blockId);
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

        // Вспомогательный метод для рекурсивного поиска ключевого слова в узлах
        private bool SearchForKeywordInNodes(XmlNode node, string keyword, string blockId = "")
        {
            bool found = false;
            int nameCount = 0;

            // Рекурсивный обход узлов
            SearchForKeywordInNodesRecursive(node, keyword, ref found, ref nameCount);

            // Для отладки
            string debugMessage = !string.IsNullOrEmpty(blockId)
                ? $"Отладка SearchForKeywordInNodes: блок '{blockId}', ищем '{keyword}', найдено NAME элементов: {nameCount}, результат: {found}"
                : $"Отладка SearchForKeywordInNodes: ищем '{keyword}', найдено NAME элементов: {nameCount}, результат: {found}";
            UpdateDashboard(debugMessage);

            return found;
        }

        // Внутренний рекурсивный метод для поиска ключевого слова
        private void SearchForKeywordInNodesRecursive(XmlNode node, string keyword, ref bool found, ref int nameCount)
        {
            if (node.NodeType == XmlNodeType.Element && node.Name == "NAME")
            {
                nameCount++;
                if (node.InnerText.Contains(keyword))
                {
                    found = true;
                }
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                SearchForKeywordInNodesRecursive(childNode, keyword, ref found, ref nameCount);
            }
        }

        // Вспомогательный метод для подсчета элементов NAME в узле
        private int CountNameNodes(XmlNode node)
        {
            int count = 0;

            if (node.NodeType == XmlNodeType.Element && node.Name == "NAME")
            {
                count++;
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                count += CountNameNodes(childNode);
            }

            return count;
        }

        // Метод для извлечения содержимого комментария
        private string ExtractCommentContent(string block)
        {
            if (block.IndexOf("-->") > 0)
            {
                return block.Substring(0, block.IndexOf("-->") + 3);
            }
            return "неизвестный комментарий";
        }



        // Метод для проверки дубликата в списке найденных вхождений
        private bool IsDuplicateEntry(string fileName, DateTime startTime, string name)
        {
            // Проверяем, есть ли уже запись с похожим временем (разница не более 2 минут) и с тем же именем
            var duplicates = _foundOccurrences.Where(occ =>
                Math.Abs((occ.StartTime - startTime).TotalMinutes) <= 2 &&
                occ.Name == name);

            foreach (var dup in duplicates)
            {
                UpdateDashboard($"Найден дубликат: {name}, время: {startTime}, существующий: {dup.StartTime}");
            }

            return duplicates.Any();
        }

        // Метод для получения уникального имени файла
        private string GetUniqueFileName(string fileName)
        {
            if (!File.Exists(fileName))
                return fileName;

            string directory = Path.GetDirectoryName(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            int counter = 1;
            string newFileName;
            do
            {
                newFileName = Path.Combine(directory, $"{fileNameWithoutExtension}_{counter}{extension}");
                counter++;
            } while (File.Exists(newFileName));

            return newFileName;
        }


    }
}
