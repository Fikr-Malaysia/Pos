!include "nsProcess.nsh"
OutFile "Setup.exe"

installDir "$PROGRAMFILES\Swdev Bali\eMailToSQL"
!define shortcutDir "$SMPROGRAMS\eMailToSQL"
RequestExecutionLevel admin


section
	SetShellVarContext all
	setOutPath $INSTDIR
	${nsProcess::KillProcess} "EmailToSQLServerExpert.exe" $R0
        ExecWait "$INSTDIR\EmailToSQLServerExpertService.exe u"
	File /r "source\*"
	writeUninstaller $INSTDIR\uninstaller.exe
	CreateDirectory "${shortcutDir}" 
	createShortCut "${shortcutDir}\eMail To SQL.lnk" "$INSTDIR\EmailToSQLServerExpert.exe"
	createShortCut "${shortcutDir}\Uninstall.lnk" "$INSTDIR\uninstaller.exe"
        ExecWait "$INSTDIR\EmailToSQLServerExpertService.exe i"
        Exec "$INSTDIR\EmailToSQLServerExpert.exe"
sectionEnd
	section "Uninstall"
	SetShellVarContext all
	ExecWait "$INSTDIR\EmailToSQLServerExpertService.exe u"
	delete "$INSTDIR\uninstaller.exe"
	RMDir /r "$INSTDIR"
	RMDir /r "${shortcutDir}"
sectionEnd