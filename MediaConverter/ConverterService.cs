using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediaConverter
{
    public sealed class ConversionProgress
    {
        public ConversionProgress(double? percent, TimeSpan processed, TimeSpan? total, string message)
        {
            Percent = percent;
            Processed = processed;
            Total = total;
            Message = message;
        }

        public double? Percent { get; }

        public TimeSpan Processed { get; }

        public TimeSpan? Total { get; }

        public string Message { get; }
    }

    public sealed class ConversionResult
    {
        public ConversionResult(string outputFile, TimeSpan elapsed)
        {
            OutputFile = outputFile;
            Elapsed = elapsed;
        }

        public string OutputFile { get; }

        public TimeSpan Elapsed { get; }
    }

    public sealed class ConverterService
    {
        private static readonly string FfmpegExecutable = ResolveExecutablePath("ffmpeg.exe", "FFMPEG_PATH");
        private static readonly string FfprobeExecutable = ResolveExecutablePath("ffprobe.exe", "FFPROBE_PATH");

        public async Task<ConversionResult> ConvertAsync(
            string inputPath,
            string outputPath,
            string format,
            string videoCodec,
            string audioCodec = "aac",
            bool useRussian = true,
            IProgress<ConversionProgress>? progress = null,
            Action<string>? logCallback = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(inputPath) || !File.Exists(inputPath))
            {
                throw new FileNotFoundException("Входной файл не найден.", inputPath);
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException("Путь для выходного файла не указан.", nameof(outputPath));
            }

            await EnsureToolExistsAsync(
                FfmpegExecutable,
                "FFmpeg не найден. Установите FFmpeg или задайте переменную FFMPEG_PATH с полным путем к ffmpeg.exe.",
                cancellationToken);

            var extension = NormalizeExtension(format);
            var outputFile = Path.ChangeExtension(outputPath, extension);
            var outputDirectory = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrWhiteSpace(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var totalDuration = await TryGetInputDurationAsync(inputPath, cancellationToken);
            var hasVideoStream = await TryDetectVideoStreamAsync(inputPath, cancellationToken);
            var qualityArgs = BuildCodecArguments(extension, videoCodec, audioCodec, hasVideoStream);
            var arguments =
                $"-y -hide_banner -loglevel warning -i \"{inputPath}\" {qualityArgs} -progress pipe:1 -nostats \"{outputFile}\"";

            logCallback?.Invoke(useRussian ? "Проверка окружения завершена." : "Environment check completed.");
            logCallback?.Invoke(useRussian
                ? $"Старт конвертации: {Path.GetFileName(inputPath)} -> {Path.GetFileName(outputFile)}"
                : $"Conversion started: {Path.GetFileName(inputPath)} -> {Path.GetFileName(outputFile)}");

            var stopwatch = Stopwatch.StartNew();
            var lastProcessed = TimeSpan.Zero;

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = FfmpegExecutable,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (_, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data))
                {
                    return;
                }

                var line = e.Data.Trim();
                const string outTimePrefix = "out_time_ms=";

                if (line.StartsWith(outTimePrefix, StringComparison.Ordinal) &&
                    long.TryParse(line.AsSpan(outTimePrefix.Length), out var outTimeMicroseconds))
                {
                    lastProcessed = TimeSpan.FromMilliseconds(outTimeMicroseconds / 1000.0);
                    var percent = CalculatePercent(lastProcessed, totalDuration);
                    progress?.Report(
                        new ConversionProgress(
                            percent,
                            lastProcessed,
                            totalDuration,
                            BuildProgressMessage(lastProcessed, totalDuration, percent, useRussian)));
                    return;
                }

                if (line.Equals("progress=end", StringComparison.OrdinalIgnoreCase))
                {
                    progress?.Report(
                        new ConversionProgress(
                            100,
                            totalDuration ?? lastProcessed,
                            totalDuration,
                            useRussian ? "Финализация результата..." : "Finalizing output..."));
                }
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    logCallback?.Invoke(e.Data.Trim());
                }
            };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                using var killRegistration = cancellationToken.Register(() => TryKillProcess(process));
                await process.WaitForExitAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                TryKillProcess(process);
                throw;
            }

            stopwatch.Stop();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"FFmpeg завершился с кодом {process.ExitCode}.");
            }

            progress?.Report(
                new ConversionProgress(
                    100,
                    totalDuration ?? lastProcessed,
                    totalDuration,
                    useRussian ? "Конвертация завершена." : "Conversion completed."));

            logCallback?.Invoke(useRussian
                ? $"Готово. Время: {stopwatch.Elapsed:mm\\:ss}"
                : $"Done. Elapsed: {stopwatch.Elapsed:mm\\:ss}");

            return new ConversionResult(outputFile, stopwatch.Elapsed);
        }

        private static string BuildCodecArguments(string extension, string videoCodec, string audioCodec, bool? hasVideoStream)
        {
            return extension switch
            {
                ".mp3" => "-vn -c:a libmp3lame -b:a 192k",
                ".aac" => "-vn -c:a aac -b:a 192k",
                ".wav" => "-vn -c:a pcm_s16le",
                ".flac" => "-vn -c:a flac",
                ".mp4" => BuildMp4Arguments(videoCodec, audioCodec, hasVideoStream),
                _ => string.Empty
            };
        }

        private static string BuildMp4Arguments(string videoCodec, string audioCodec, bool? hasVideoStream)
        {
            var audioArgs = GetMp4AudioArgs(audioCodec);

            if (hasVideoStream == false)
            {
                return $"-vn {audioArgs} -movflags +faststart";
            }

            if (hasVideoStream == true)
            {
                return $"{GetVideoCodecArgs(videoCodec)} {audioArgs} -movflags +faststart";
            }

            return $"{audioArgs} -movflags +faststart";
        }

        private static string GetMp4AudioArgs(string audioCodec)
        {
            return audioCodec.Trim().ToLowerInvariant() switch
            {
                "copy" => "-c:a copy",
                "mp3" => "-c:a libmp3lame -b:a 192k",
                "flac" => "-c:a flac",
                _ => "-c:a aac -b:a 192k"
            };
        }

        private static string GetVideoCodecArgs(string videoCodec)
        {
            return videoCodec.Trim().ToLowerInvariant() switch
            {
                "h265" => "-c:v libx265 -preset medium -crf 26",
                "copy" => "-c:v copy",
                _ => "-c:v libx264 -preset medium -crf 24"
            };
        }

        private static string NormalizeExtension(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return ".out";
            }

            var normalized = format.Trim().ToLowerInvariant();
            if (!normalized.StartsWith(".", StringComparison.Ordinal))
            {
                normalized = "." + normalized;
            }

            return normalized;
        }

        private static string ResolveExecutablePath(string executableName, string overrideEnvVar)
        {
            var overridePath = Environment.GetEnvironmentVariable(overrideEnvVar);
            if (!string.IsNullOrWhiteSpace(overridePath))
            {
                var trimmed = overridePath.Trim().Trim('"');
                if (File.Exists(trimmed))
                {
                    return trimmed;
                }
            }

            if (TryFindInPath(executableName, out var fromPath))
            {
                return fromPath;
            }

            foreach (var candidate in GetKnownExecutablePaths(executableName))
            {
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            return Path.GetFileNameWithoutExtension(executableName);
        }

        private static bool TryFindInPath(string executableName, out string fullPath)
        {
            var pathValue = Environment.GetEnvironmentVariable("PATH");
            if (!string.IsNullOrWhiteSpace(pathValue))
            {
                foreach (var segment in pathValue.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
                {
                    var folder = segment.Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(folder))
                    {
                        continue;
                    }

                    var candidate = Path.Combine(folder, executableName);
                    if (File.Exists(candidate))
                    {
                        fullPath = candidate;
                        return true;
                    }
                }
            }

            fullPath = string.Empty;
            return false;
        }

        private static IEnumerable<string> GetKnownExecutablePaths(string executableName)
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var appBase = AppContext.BaseDirectory;

            if (!string.IsNullOrWhiteSpace(userProfile))
            {
                yield return Path.Combine(userProfile, "scoop", "shims", executableName);
                yield return Path.Combine(userProfile, "scoop", "apps", "ffmpeg", "current", "bin", executableName);
            }

            if (!string.IsNullOrWhiteSpace(localAppData))
            {
                yield return Path.Combine(localAppData, "Microsoft", "WinGet", "Packages", "Gyan.FFmpeg_Microsoft.Winget.Source_8wekyb3d8bbwe", "ffmpeg", "bin", executableName);
            }

            if (!string.IsNullOrWhiteSpace(programData))
            {
                yield return Path.Combine(programData, "scoop", "shims", executableName);
                yield return Path.Combine(programData, "scoop", "apps", "ffmpeg", "current", "bin", executableName);
                yield return Path.Combine(programData, "chocolatey", "bin", executableName);
            }

            yield return Path.Combine("C:\\ffmpeg", "bin", executableName);
            yield return Path.Combine("C:\\Program Files", "ffmpeg", "bin", executableName);
            yield return Path.Combine("C:\\Program Files (x86)", "ffmpeg", "bin", executableName);

            // Allow shipping ffmpeg with app folder: {app}/ffmpeg/bin/ffmpeg.exe.
            yield return Path.Combine(appBase, "ffmpeg", "bin", executableName);
            yield return Path.Combine(appBase, executableName);
        }

        private static async Task EnsureToolExistsAsync(string toolName, string failMessage, CancellationToken cancellationToken)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = toolName,
                Arguments = "-version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = Process.Start(startInfo);
                if (process is null)
                {
                    throw new InvalidOperationException(failMessage);
                }

                await process.WaitForExitAsync(cancellationToken);
                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException(failMessage);
                }
            }
            catch (Exception ex) when (ex is Win32Exception or InvalidOperationException)
            {
                throw new InvalidOperationException(failMessage, ex);
            }
        }

        private static async Task<TimeSpan?> TryGetInputDurationAsync(string inputPath, CancellationToken cancellationToken)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = FfprobeExecutable,
                Arguments = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{inputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = Process.Start(startInfo);
                if (process is null)
                {
                    return null;
                }

                var output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync(cancellationToken);

                if (process.ExitCode != 0)
                {
                    return null;
                }

                if (!double.TryParse(output.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var seconds) ||
                    seconds <= 0)
                {
                    return null;
                }

                return TimeSpan.FromSeconds(seconds);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return null;
            }
        }

        private static async Task<bool?> TryDetectVideoStreamAsync(string inputPath, CancellationToken cancellationToken)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = FfprobeExecutable,
                Arguments = $"-v error -select_streams v:0 -show_entries stream=codec_type -of default=noprint_wrappers=1:nokey=1 \"{inputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = Process.Start(startInfo);
                if (process is null)
                {
                    return null;
                }

                var output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync(cancellationToken);

                if (process.ExitCode != 0)
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(output))
                {
                    return false;
                }

                return output.Trim().Contains("video", StringComparison.OrdinalIgnoreCase);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                return null;
            }
        }

        private static double? CalculatePercent(TimeSpan processed, TimeSpan? total)
        {
            if (!total.HasValue || total.Value <= TimeSpan.Zero)
            {
                return null;
            }

            var percent = processed.TotalMilliseconds / total.Value.TotalMilliseconds * 100;
            return Math.Clamp(percent, 0, 100);
        }

        private static string BuildProgressMessage(TimeSpan processed, TimeSpan? total, double? percent, bool useRussian)
        {
            if (percent.HasValue && total.HasValue)
            {
                return useRussian
                    ? $"Конвертация {percent.Value:0}% ({processed:hh\\:mm\\:ss} / {total.Value:hh\\:mm\\:ss})"
                    : $"Converting {percent.Value:0}% ({processed:hh\\:mm\\:ss} / {total.Value:hh\\:mm\\:ss})";
            }

            return useRussian
                ? $"Конвертация... ({processed:hh\\:mm\\:ss})"
                : $"Converting... ({processed:hh\\:mm\\:ss})";
        }

        private static void TryKillProcess(Process process)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                }
            }
            catch
            {
                // Игнорируем, потому что процесс мог завершиться между проверкой и попыткой остановки.
            }
        }
    }
}
