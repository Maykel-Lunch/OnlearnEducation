[Setup]
AppName=OnlearnEducation
AppVersion=1.0
DefaultDirName={pf}\OnlearnEducation
DefaultGroupName=OnlearnEducation
OutputBaseFilename=OnlearnEducationInstaller

[Files]
Source: "net8.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "OnLearnEducation.sql"; DestDir: "{app}\db"; Flags: ignoreversion
Source: "import_db.bat"; DestDir: "{app}\db"; Flags: ignoreversion

[Icons]
Name: "{group}\OnlearnEducation"; Filename: "{app}\OnlearnEducation.exe"

[Run]
; Optionally run the batch file to import the database
; Filename: "{app}\db\import_db.bat"; Flags: postinstall runascurrentuser