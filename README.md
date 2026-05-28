# Media Converter

Настольное приложение для Windows для конвертации аудио- и видеофайлов, построенное на .NET 8 WinForms с FFmpeg в качестве бэкенда.

Разработано в рамках дипломного проекта по дисциплине *«Технология разработки программного обеспечения»*.

## Возможности

- **Drag-and-drop** — перетащите файл прямо в окно приложения
- **Форматы вывода** — MP4, MP3, WAV, AAC, FLAC
- **Выбор кодека** — H.264, H.265 или потоковое копирование для видео; AAC, MP3, FLAC или копирование для аудио
- **Пресеты** — готовые профили в одно нажатие: Web, High Quality, Remux, Audio MP3, Lossless FLAC
- **Прогресс в реальном времени** — процент выполнения через FFmpeg `-progress pipe:1`
- **Отмена** — остановка конвертации без потери результата
- **Лог выполнения** — цветные записи с временными метками
- **Итоговый диалог** — сводка по завершении с кнопкой открытия папки
- **Двуязычный интерфейс** — переключение между русским и английским без перезапуска
- **Тёмная тема** — современный indigo/purple дизайн

## Технологии

- Язык: **C# (.NET 8.0)**
- Интерфейс: **Windows Forms**
- Медиабэкенд: **FFmpeg**
- IDE: **Visual Studio 2022**

## Архитектура

```
MediaConverter/
├── Program.cs               # Точка входа
├── MainForm.cs              # Логика главного окна, конвертация, лог
├── MainForm.Designer.cs     # Разметка WinForms
├── ConverterService.cs      # Обёртка над процессом FFmpeg
└── SuccessForm.cs           # Диалог с итогами конвертации
```

`ConverterService` не зависит от UI и взаимодействует с `MainForm` через `IProgress<ConversionProgress>`.

## Установка

1. Скачайте установщик из папки `/InstallBuild`
2. Запустите `MediaConverterSetup.exe` и следуйте мастеру установки
3. Установите FFmpeg (см. ниже) — без него конвертация невозможна

## Требования

| Компонент | Версия |
|-----------|--------|
| Windows | 10 и выше (64-bit) |
| .NET Runtime | 8.0 |
| FFmpeg | 4.4+ (включая `ffprobe`) |

### Установка FFmpeg

```cmd
winget install Gyan.FFmpeg
```

или через Scoop / Chocolatey:

```cmd
scoop install ffmpeg
choco install ffmpeg
```

Проверьте установку: `ffmpeg -version`

## Сборка из исходников

```cmd
dotnet build MediaConverter.sln
dotnet run --project MediaConverter
```

## Поиск FFmpeg приложением

Приложение ищет `ffmpeg.exe` и `ffprobe.exe` в следующем порядке:

1. Переменные среды `FFMPEG_PATH` / `FFPROBE_PATH`
2. Системный `PATH`
3. Стандартные пути: scoop shims, WinGet packages, Chocolatey, `C:\ffmpeg\bin\`
4. Директория приложения (`{exe}/ffmpeg/bin/`)

## Скриншоты

![Главное окно](image/README/1771009700083.png)
