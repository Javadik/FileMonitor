using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor
{
    internal class OldFile
    {
        public static List<string> richListOld { get; set; } = new List<string>();
        public static void DeleteFilesOlderThan(string directoryPath, TimeSpan maxAge)
        {

            try
            {
                // Обрабатываем все файлы во всех поддиректориях
                foreach (string file in Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        if (fileInfo.LastWriteTimeUtc < DateTime.UtcNow - maxAge)
                        {
                            fileInfo.Delete();
                            richListOld.Add($"Удаление: {file} (Последнее изменение: {fileInfo.LastWriteTime})");
                        }
                    }
                    catch (Exception ex)
                    {
                        richListOld.Add($"Ошибка при обработке файла {file}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                richListOld.Add($"Ошибка при сканировании директории: {ex.Message}");
            }
        }

        public static TimeSpan GetMaxAge(ref string tbDays)
        {
            if (!int.TryParse(tbDays, out int days) || days <= 0) {
                tbDays = 90.ToString();
                return TimeSpan.FromDays(90);
            }
            days = days < 1 ? 1 : (days > 3650 ? 3650 : days);
            tbDays = days.ToString();
            return TimeSpan.FromDays(days);
        }
    }
}
