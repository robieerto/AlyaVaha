; Define the application information
[Setup]
AppName=AlyaVaha
AppVersion=1.0
DefaultDirName={commonpf}\AlyaVaha
DefaultGroupName=AlyaVaha
OutputDir=.
OutputBaseFilename=AlyaVahaInstaller
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

; Define the directories to be created
[Dirs]
Name: "C:\sqlite"

; Define the files to be installed
[Files]
Source: "C:\Users\rober\source\repos\AlyaVaha\AlyaVaha\bin\Publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
; Source: "path\to\your\cmd\script\script.cmd"; DestDir: "{app}"; Flags: ignoreversion

; Define the icons (shortcuts)
[Icons]
Name: "{group}\AlyaVaha"; Filename: "{app}\AlyaVaha.exe"
Name: "{commondesktop}\AlyaVaha"; Filename: "{app}\AlyaVaha.exe"; Tasks: desktopicon

; Define the tasks
[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

; Define the run section to execute the CMD script
[Run]
; Filename: "{app}\script.cmd"; Flags: runhidden
Filename: "{app}\AlyaVaha.exe"; Description: "{cm:LaunchProgram,AlyaVaha}"; Flags: runascurrentuser nowait;

; Define the messages
[Messages]