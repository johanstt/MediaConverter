#define MyAppName      "Media Converter"
#define MyAppVersion   "1.0.0"
#define MyAppPublisher "Ivanenko Egor"
#define MyAppExeName   "MediaConverter.exe"
#define AppDir         "InstallBuild\app"

[Setup]
AppId={{A7C3B2D1-4E5F-4A2B-9C8D-1E2F3A4B5C6D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL=https://github.com
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=InstallBuild
OutputBaseFilename=MediaConverterSetup
SetupIconFile=
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
WizardSizePercent=120
PrivilegesRequired=admin
MinVersion=10.0
ArchitecturesInstallIn64BitMode=x64compatible
UninstallDisplayName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription={#MyAppName} Setup

[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon";    Description: "{cm:CreateDesktopIcon}";    GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1

[Files]
Source: "{#AppDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}";              Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}";        Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
function FFmpegFound(): Boolean;
begin
  Result :=
    FileExists('C:\ffmpeg\bin\ffmpeg.exe') or
    FileExists('C:\Program Files\ffmpeg\bin\ffmpeg.exe') or
    FileExists('C:\Program Files (x86)\ffmpeg\bin\ffmpeg.exe') or
    FileExists(ExpandConstant('{app}\ffmpeg\bin\ffmpeg.exe'));
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  Msg: String;
begin
  if CurStep = ssPostInstall then
  begin
    if not FFmpegFound() then
    begin
      Msg :=
        'Media Converter требует FFmpeg для работы.' + #13#10 + #13#10 +
        'Установите FFmpeg одним из способов:' + #13#10 + #13#10 +
        '  winget install Gyan.FFmpeg' + #13#10 +
        '  scoop install ffmpeg' + #13#10 +
        '  choco install ffmpeg' + #13#10 + #13#10 +
        'После установки FFmpeg приложение будет готово к работе.';
      MsgBox(Msg, mbInformation, MB_OK);
    end;
  end;
end;
