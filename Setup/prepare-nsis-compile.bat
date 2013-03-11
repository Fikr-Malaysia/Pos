@echo off
del source\*.* /S /F /Q
mkdir source
copy "..\SupportAlerter\bin\release\EmailToSQLServerExpert.exe" source
copy "..\SupportAlerter\bin\release\OpenPop.dll" source
copy "..\SupportAlerter\bin\release\EmailToSQLServerExpertLibrary.dll" source
mkdir source\icon
copy "..\SupportAlerter\bin\release\icon\sms-32.ico" "source\icon\" 
copy "..\SupportAlerterService\bin\release\EmailToSQLServerExpertService.exe" source
copy "..\SupportAlerterService\bin\release\InstallUtil.exe" source

