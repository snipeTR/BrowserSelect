; BrowserSelect installer (Windows 64-bit)
;
; Build (from the BrowserSelect project folder, after a Release|x64 build):
;   makensis /DVERSION=1.4.2.0 installer.nsi
; Optional defines:
;   /DBUILD_DIR=<path to build output>   (default: .\bin\x64\Release)
;   /DOUTFILE=<installer file name>      (default: BrowserSelect-<VERSION>-x64-Setup.exe)
;
; Languages: English and Turkish; NSIS picks the one matching the Windows display language (English otherwise).
; This file is saved as UTF-8 with BOM so makensis reads the Turkish texts correctly on any code page.
;
; If BrowserSelect.exe is running (for the current user), setup and uninstall ask for permission to close it
; (WM_CLOSE first, then forced). Uses only tasklist/taskkill via the built-in nsExec plugin, no extra plugins.

Unicode true

!ifndef VERSION
  !define VERSION "1.4.2.0"
!endif
!ifndef BUILD_DIR
  !define BUILD_DIR ".\bin\x64\Release"
!endif
!ifndef OUTFILE
  !define OUTFILE "BrowserSelect-${VERSION}-x64-Setup.exe"
!endif

Name "BrowserSelect"
Caption "$(BS_Caption)"
Icon "bs.ico"
UninstallIcon "bs.ico"
OutFile "${OUTFILE}"

InstallDir "$LOCALAPPDATA\BrowserSelect"
InstallDirRegKey HKCU "Software\BrowserSelect" ""

RequestExecutionLevel user

VIProductVersion "${VERSION}"
VIAddVersionKey "ProductName" "BrowserSelect"
VIAddVersionKey "FileDescription" "BrowserSelect 64-bit Installer"
VIAddVersionKey "FileVersion" "${VERSION}"
VIAddVersionKey "ProductVersion" "${VERSION}"
VIAddVersionKey "LegalCopyright" "GPL-2.0"

;--------------------------------
;Interface Settings
  !include "MUI2.nsh"
  !include "x64.nsh"
  !include "LogicLib.nsh"
  !define MUI_ABORTWARNING
  !define MUI_ICON "bs.ico"
  !define MUI_UNICON "bs.ico"

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_LICENSE "./License.txt"
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES

  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "English"
  !insertmacro MUI_LANGUAGE "Turkish"

  LangString BS_Caption ${LANG_ENGLISH} "BrowserSelect ${VERSION} (64-bit) Installation"
  LangString BS_Caption ${LANG_TURKISH} "BrowserSelect ${VERSION} (64-bit) Kurulumu"

  LangString BS_Requires64 ${LANG_ENGLISH} "This build of BrowserSelect requires 64-bit Windows."
  LangString BS_Requires64 ${LANG_TURKISH} "BrowserSelect'in bu sürümü 64-bit Windows gerektirir."

  LangString BS_AppRunning ${LANG_ENGLISH} "BrowserSelect is currently running.$\r$\n$\r$\nSetup has to close it to continue. Do you want to close BrowserSelect now?"
  LangString BS_AppRunning ${LANG_TURKISH} "BrowserSelect şu anda çalışıyor.$\r$\n$\r$\nKurulumun devam edebilmesi için programın kapatılması gerekiyor. BrowserSelect şimdi kapatılsın mı?"

  LangString BS_CloseRequired ${LANG_ENGLISH} "Setup cannot continue while BrowserSelect is running.$\r$\n$\r$\nPlease close the program and restart the setup."
  LangString BS_CloseRequired ${LANG_TURKISH} "BrowserSelect çalışırken kurulum devam edemez.$\r$\n$\r$\nLütfen programı kapatıp kurulumu yeniden başlatın."

  LangString BS_UnAppRunning ${LANG_ENGLISH} "BrowserSelect is currently running.$\r$\n$\r$\nIt has to be closed before it can be uninstalled. Do you want to close BrowserSelect now?"
  LangString BS_UnAppRunning ${LANG_TURKISH} "BrowserSelect şu anda çalışıyor.$\r$\n$\r$\nKaldırılabilmesi için programın kapatılması gerekiyor. BrowserSelect şimdi kapatılsın mı?"

  LangString BS_UnCloseRequired ${LANG_ENGLISH} "BrowserSelect cannot be uninstalled while it is running.$\r$\n$\r$\nPlease close the program and start the uninstall again."
  LangString BS_UnCloseRequired ${LANG_TURKISH} "BrowserSelect çalışırken kaldırılamaz.$\r$\n$\r$\nLütfen programı kapatıp kaldırma işlemini yeniden başlatın."

  LangString BS_CloseFailed ${LANG_ENGLISH} "BrowserSelect could not be closed.$\r$\n$\r$\nPlease close the program manually and restart the setup."
  LangString BS_CloseFailed ${LANG_TURKISH} "BrowserSelect kapatılamadı.$\r$\n$\r$\nLütfen programı elle kapatıp kurulumu yeniden başlatın."

  LangString BS_UnCloseFailed ${LANG_ENGLISH} "BrowserSelect could not be closed.$\r$\n$\r$\nPlease close the program manually and start the uninstall again."
  LangString BS_UnCloseFailed ${LANG_TURKISH} "BrowserSelect kapatılamadı.$\r$\n$\r$\nLütfen programı elle kapatıp kaldırma işlemini yeniden başlatın."

;--------------------------------
;Closing a running BrowserSelect.exe (installer and uninstaller)

; checks whether BrowserSelect.exe runs for the current user; pushes 1 (running) or 0
!macro BS_DEFINE_IS_RUNNING PREFIX
Function ${PREFIX}BSIsRunning
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4
  ReadEnvStr $0 USERNAME
  ; CSV without header: a match starts with "BrowserSelect.exe",... ; no match prints a (localized) INFO line
  nsExec::ExecToStack '"$SYSDIR\tasklist.exe" /NH /FO CSV /FI "IMAGENAME eq BrowserSelect.exe" /FI "USERNAME eq $0"'
  Pop $1 ; exit code ("error" if tasklist could not be started)
  Pop $2 ; output
  StrCpy $0 0
  ${If} $1 != "error"
    ; search "BrowserSelect.exe" in the output (StrCmp is case-insensitive)
    StrLen $3 $2
    StrCpy $1 0
    ${DoWhile} $1 < $3
      StrCpy $4 $2 17 $1
      ${If} $4 == "BrowserSelect.exe"
        StrCpy $0 1
        ${ExitDo}
      ${EndIf}
      IntOp $1 $1 + 1
    ${Loop}
  ${EndIf}
  Pop $4
  Pop $3
  Pop $2
  Pop $1
  Exch $0
FunctionEnd
!macroend
!insertmacro BS_DEFINE_IS_RUNNING ""
!insertmacro BS_DEFINE_IS_RUNNING "un."

; asks for permission to close a running BrowserSelect; closes it (WM_CLOSE, then forced) or aborts.
; ASK = LangString with the question, REFUSED = LangString shown when the user says No,
; FAILED = LangString shown when it could not be closed.
; In silent mode (/S) the program is closed without asking.
!macro BS_CLOSE_RUNNING PREFIX ASK REFUSED FAILED
  Push $0
  Push $1
  Call ${PREFIX}BSIsRunning
  Pop $0
  ${If} $0 == 1
    MessageBox MB_YESNO|MB_ICONQUESTION|MB_DEFBUTTON1 "$(${ASK})" /SD IDYES IDYES +3
      MessageBox MB_OK|MB_ICONEXCLAMATION "$(${REFUSED})" /SD IDOK
      Abort
    ReadEnvStr $1 USERNAME
    ; 1) polite: WM_CLOSE to its windows, wait up to 5 s
    nsExec::Exec '"$SYSDIR\taskkill.exe" /IM BrowserSelect.exe /FI "USERNAME eq $1"'
    Pop $0
    StrCpy $1 0
    ${Do}
      Sleep 500
      Call ${PREFIX}BSIsRunning
      Pop $0
      IntOp $1 $1 + 1
      ${IfThen} $0 != 1 ${|} ${ExitDo} ${|}
    ${LoopUntil} $1 >= 10
    ; 2) still running: force
    ${If} $0 == 1
      ReadEnvStr $1 USERNAME
      nsExec::Exec '"$SYSDIR\taskkill.exe" /F /IM BrowserSelect.exe /FI "USERNAME eq $1"'
      Pop $0
      StrCpy $1 0
      ${Do}
        Sleep 500
        Call ${PREFIX}BSIsRunning
        Pop $0
        IntOp $1 $1 + 1
        ${IfThen} $0 != 1 ${|} ${ExitDo} ${|}
      ${LoopUntil} $1 >= 10
      ${If} $0 == 1
        MessageBox MB_OK|MB_ICONSTOP "$(${FAILED})" /SD IDOK
        Abort
      ${EndIf}
    ${EndIf}
    ; give Windows a moment to release the file handles
    Sleep 500
  ${EndIf}
  Pop $1
  Pop $0
!macroend

;--------------------------------
;64-bit only

Function .onInit
  ${IfNot} ${RunningX64}
    MessageBox MB_OK|MB_ICONSTOP "$(BS_Requires64)" /SD IDOK
    Abort
  ${EndIf}
  SetRegView 64
  ; BrowserSelect must not be running while its files are replaced
  !insertmacro BS_CLOSE_RUNNING "" BS_AppRunning BS_CloseRequired BS_CloseFailed
FunctionEnd

Function un.onInit
  SetRegView 64
  !insertmacro BS_CLOSE_RUNNING "un." BS_UnAppRunning BS_UnCloseRequired BS_UnCloseFailed
FunctionEnd

;--------------------------------
;Installer Sections

Section "BrowserSelect" SecMain

  ; it may have been started again while the setup pages were open
  !insertmacro BS_CLOSE_RUNNING "" BS_AppRunning BS_CloseRequired BS_CloseFailed

  SetOutPath "$INSTDIR"

  File "/oname=BrowserSelect.exe" "${BUILD_DIR}\BrowserSelect.exe"
  File "/oname=BrowserSelect.exe.config" "${BUILD_DIR}\BrowserSelect.exe.config"
  File "/oname=Newtonsoft.Json.dll" "${BUILD_DIR}\Newtonsoft.Json.dll"
  File "/oname=License.txt" ".\License.txt"
  ;translations: satellite assemblies <culture>\BrowserSelect.resources.dll (e.g. tr\BrowserSelect.resources.dll)
  File /nonfatal /r "${BUILD_DIR}\BrowserSelect.resources.dll"
  CreateShortCut "$SMPROGRAMS\BrowserSelect.lnk" "$INSTDIR\BrowserSelect.exe"

  ;Store installation folder
  WriteRegStr HKCU "Software\BrowserSelect" "" $INSTDIR
  ;For control panel uninstall
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\BrowserSelect" \
                 "DisplayName" "BrowserSelect -- select browser dynamically"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\BrowserSelect" \
                 "DisplayVersion" "${VERSION}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\BrowserSelect" \
                 "DisplayIcon" "$INSTDIR\BrowserSelect.exe,0"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\BrowserSelect" \
                 "InstallLocation" "$INSTDIR"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\BrowserSelect" \
                 "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\BrowserSelect" \
                 "NoModify" 1
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\BrowserSelect" \
                 "NoRepair" 1
  ;for register as default browser
  ;create entry in startmenuinternet
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE" \
                 "" "Browser Select"
;add capabilities
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities" \
                 "ApplicationName" "BrowserSelect"
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities" \
                 "ApplicationDescription" "Choose a Browser dynamically."
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities" \
                 "ApplicationIcon" "$INSTDIR\BrowserSelect.exe,0"

  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\StartMenu" \
                 "StartMenuInternet" "BROWSERSELECT.EXE"

  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\URLAssociations" \
                 "http" "bselectURL"
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\URLAssociations" \
                 "https" "bselectURL"
;add icon and command
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\DefaultIcon" \
                 "" "$INSTDIR\BrowserSelect.exe,0"
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\shell\open\command" \
                 "" "$\"$INSTDIR\BrowserSelect.exe$\""
;register capablities
  WriteRegStr HKCU "Software\RegisteredApplications" \
                 "BrowserSelect" "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities"
;register handler
  WriteRegStr HKCU "Software\Classes\bselectURL" \
                 "" "BrowserSelect Url"
  WriteRegStr HKCU "Software\Classes\bselectURL\shell\open\command" \
                 "" "$\"$INSTDIR\BrowserSelect.exe$\" $\"%1$\""
;file associations (.html, .url, ...): offered in Default apps and the "Open with" menu
  WriteRegStr HKCU "Software\Classes\bselectHTML" "" "BrowserSelect Document"
  WriteRegStr HKCU "Software\Classes\bselectHTML\DefaultIcon" "" "$INSTDIR\BrowserSelect.exe,0"
  WriteRegStr HKCU "Software\Classes\bselectHTML\shell\open\command" \
                 "" "$\"$INSTDIR\BrowserSelect.exe$\" $\"%1$\""
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\FileAssociations" \
                 ".htm" "bselectHTML"
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\FileAssociations" \
                 ".html" "bselectHTML"
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\FileAssociations" \
                 ".shtml" "bselectHTML"
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\FileAssociations" \
                 ".xht" "bselectHTML"
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\FileAssociations" \
                 ".xhtml" "bselectHTML"
  WriteRegStr HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE\Capabilities\FileAssociations" \
                 ".url" "bselectHTML"
  WriteRegStr HKCU "Software\Classes\.htm\OpenWithProgids" "bselectHTML" ""
  WriteRegStr HKCU "Software\Classes\.html\OpenWithProgids" "bselectHTML" ""
  WriteRegStr HKCU "Software\Classes\.shtml\OpenWithProgids" "bselectHTML" ""
  WriteRegStr HKCU "Software\Classes\.xht\OpenWithProgids" "bselectHTML" ""
  WriteRegStr HKCU "Software\Classes\.xhtml\OpenWithProgids" "bselectHTML" ""
  WriteRegStr HKCU "Software\Classes\.url\OpenWithProgids" "bselectHTML" ""
  System::Call 'shell32::SHChangeNotify(i 0x08000000, i 0, p 0, p 0)'

  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  !insertmacro BS_CLOSE_RUNNING "un." BS_UnAppRunning BS_UnCloseRequired BS_UnCloseFailed

  Delete "$INSTDIR\Uninstall.exe"
  Delete "$INSTDIR\BrowserSelect.exe"
  Delete "$INSTDIR\BrowserSelect.exe.config"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\License.txt"
  Delete "$SMPROGRAMS\BrowserSelect.lnk"
  ;translations (<culture>\BrowserSelect.resources.dll)
  FindFirst $0 $1 "$INSTDIR\*"
  ${DoWhile} $1 != ""
    ${If} ${FileExists} "$INSTDIR\$1\BrowserSelect.resources.dll"
      Delete "$INSTDIR\$1\BrowserSelect.resources.dll"
      RMDir "$INSTDIR\$1"
    ${EndIf}
    FindNext $0 $1
  ${Loop}
  FindClose $0

  ; todo: remove user.conf file(s) after asking user
  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\BrowserSelect"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\BrowserSelect"
  DeleteRegKey HKCU "Software\Clients\StartMenuInternet\BROWSERSELECT.EXE"
  DeleteRegValue  HKCU "Software\RegisteredApplications" "BrowserSelect"
  DeleteRegKey HKCU "Software\Classes\bselectURL"
  DeleteRegKey HKCU "Software\Classes\bselectHTML"
  DeleteRegValue HKCU "Software\Classes\.htm\OpenWithProgids" "bselectHTML"
  DeleteRegValue HKCU "Software\Classes\.html\OpenWithProgids" "bselectHTML"
  DeleteRegValue HKCU "Software\Classes\.shtml\OpenWithProgids" "bselectHTML"
  DeleteRegValue HKCU "Software\Classes\.xht\OpenWithProgids" "bselectHTML"
  DeleteRegValue HKCU "Software\Classes\.xhtml\OpenWithProgids" "bselectHTML"
  DeleteRegValue HKCU "Software\Classes\.url\OpenWithProgids" "bselectHTML"
  System::Call 'shell32::SHChangeNotify(i 0x08000000, i 0, p 0, p 0)'

SectionEnd
