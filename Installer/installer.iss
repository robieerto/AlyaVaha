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
Source: "D:\Programs\.net\windowsdesktop-runtime-8.0.7-win-x64.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall
Source: "D:\Programs\.net\aspnetcore-runtime-8.0.7-win-x64.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall

; Define the icons (shortcuts)
[Icons]
Name: "{group}\AlyaVaha"; Filename: "{app}\AlyaVaha.exe"
Name: "{commondesktop}\AlyaVaha"; Filename: "{app}\AlyaVaha.exe"; Tasks: desktopicon

; Define the tasks
[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

; Define the run section to execute the CMD script
[Run]
Filename: "{tmp}\windowsdesktop-runtime-8.0.7-win-x64.exe"; Parameters: "/install /quiet /norestart"; Flags: waituntilterminated
Filename: "{tmp}\aspnetcore-runtime-8.0.7-win-x64.exe"; Parameters: "/install /quiet /norestart"; Flags: waituntilterminated
Filename: "{app}\AlyaVaha.exe"; Description: "{cm:LaunchProgram,AlyaVaha}"; Flags: runascurrentuser nowait postinstall; Check: "ShouldLaunchMyApp"

; Define the uninstall delete section to remove the installed folder
[UninstallDelete]
Type: filesandordirs; Name: "{app}"

; Pascal Script to add a custom page
[Code]
function ShouldLaunchMyApp: Boolean;
begin
  Result := True;
end;

function IsNumeric(S: String): Boolean;
var
  I: Integer;
begin
  Result := True;
  if (Length(S) = 0) then
    begin
      Result := False;
      Exit;
    end;
  for I := 1 to Length(S) do
  begin
    if (S[I] < '0') or (S[I] > '9') then
    begin
      Result := False;
      Exit;
    end;
  end;
end;

var
  NumericPage: TWizardPage;
  NumericEdit: TEdit;

procedure InitializeWizard;
begin
  NumericPage := CreateCustomPage(wpWelcome, 'Počet zariadení', 'Zadajte počet zariadení');
  NumericEdit := TEdit.Create(NumericPage);
  NumericEdit.Parent := NumericPage.Surface;
  NumericEdit.Left := 10;
  NumericEdit.Top := 10;
  NumericEdit.Width := NumericPage.SurfaceWidth - 20;
  NumericEdit.Text := '1'; // Set default value to 1
end;

function NextButtonClick(CurPageID: Integer) : Boolean;
var
  NumericValue: String;
begin
  Result := True
  if CurPageID = NumericPage.ID then
  begin
    NumericValue := NumericEdit.Text;
    if not IsNumeric(NumericValue) then
    begin
      MsgBox('Zadajte validný počet zariadení', mbError, MB_OK);
      Result := False
    end;
  end;
end;

procedure CurPageChanged(CurPageID: Integer);
var
  NumericValue: String;
begin
  if CurPageID = wpFinished then
  begin
    NumericValue := NumericEdit.Text;
    SaveStringToFile(ExpandConstant('{app}\.initConfig'), NumericValue, True);
  end;
end;