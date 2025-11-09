using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MediaConverter
{
    public class ConverterService
    {
        /// <summary>
        /// Асинхронно конвертирует медиафайл с помощью FFmpeg.
        /// </summary>
        /// <param name="inputPath">Путь к исходному файлу</param>
        /// <param name="outputPath">Путь для сохранения результата</param>
        /// <param name="format">Выбранный формат (например, mp3 или mp4)</param>
        /// <param name="logCallback">Метод для вывода логов в интерфейс</param>
        public async Task ConvertAsync(string inputPath, string outputPath, string format, Action<string>? logCallback)
        {
            if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
                throw new FileNotFoundException("Файл не найден!", inputPath);

            if (string.IsNullOrWhiteSpace(outputPath))
                throw new ArgumentException("Путь вывода не указан.");

            string ffmpegPath = "ffmpeg"; // ffmpeg должен быть добавлен в PATH
            logCallback?.Invoke("🔍 Проверка FFmpeg...");

            // Проверяем наличие FFmpeg
            var checkInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = "-version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var checkProcess = Process.Start(checkInfo);
                if (checkProcess != null)
                    await checkProcess.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("FFmpeg не найден. Убедитесь, что он установлен и добавлен в PATH.", ex);
            }

            logCallback?.Invoke("✅ FFmpeg найден.");
            logCallback?.Invoke($"🎬 Начало конвертации: {Path.GetFileName(inputPath)}");

            // Формируем выходное имя файла
            string extension = format switch
            {
                string s when s.Contains("mp4") => ".mp4",
                string s when s.Contains("mp3") => ".mp3",
                string s when s.Contains("wav") => ".wav",
                string s when s.Contains("aac") => ".aac",
                string s when s.Contains("flac") => ".flac",
                _ => ".out"
            };

            string outputFile = Path.ChangeExtension(outputPath, extension);

            // Формируем аргументы для FFmpeg
            string args = $"-y -i \"{inputPath}\" \"{outputFile}\"";

            var psi = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var stopwatch = Stopwatch.StartNew();

            using var process = new Process { StartInfo = psi };

            process.OutputDataReceived += (_, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    logCallback?.Invoke($"🔹 {e.Data}");
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    logCallback?.Invoke($"🔸 {e.Data}");
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            stopwatch.Stop();

            logCallback?.Invoke($"✅ Готово! Файл сохранён как: {Path.GetFileName(outputFile)}");
            logCallback?.Invoke($"⏱ Время выполнения: {stopwatch.Elapsed:mm\\:ss}");
        }
    }
}
