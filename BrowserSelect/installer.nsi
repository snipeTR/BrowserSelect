; BrowserSelect installer (Windows 64-bit)
;
; Build (from the BrowserSelect project folder, after a Release|x64 build):
;   makensis /DVERSION=1.4.1.0 installer.nsi
; Optional defines:
;   /DBUILD_DIR=<path to build output>   (default: .\bin\x64\Release)
;   /DOUTFILE=<installer file name>      (default: BrowserSelect-<VERSION>-x64-Setup.exe)

Unicode true

!ifndef VERSION
  !define VERSION "1.4.1.0"
!endif
!ifndef BUILD_DIR
  !define BUILD_DIR ".\bin\x64\Release"
!endif
!ifndef OUTFILE
  !define OUTFILE "BrowserSelect-${VERSION}-x64-Setup.exe"
!endif

Name "BrowserSelect"
Caption "BrowserSelect ${VERSION} (64-bit) Installation"
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

;--------------------------------
;64-bit only

Function .onInit
  ${IfNot} ${RunningX64}
    MessageBox MB_OK|MB_ICONSTOP "This build of BrowserSelect requires 64-bit Windows."
    Abort
  ${EndIf}
  SetRegView 64
FunctionEnd

Function un.onInit
  SetRegView 64
FunctionEnd

;--------------------------------
;Installer Sections

Section "BrowserSelect" SecMain

  SetOutPath "$INSTDIR"

  File "/oname=BrowserSelect.exe" "${BUILD_DIR}\BrowserSelect.exe"
  File "/oname=BrowserSelect.exe.config" "${BUILD_DIR}\BrowserSelect.exe.config"
  File "/oname=Newtonsoft.Json.dll" "${BUILD_DIR}\Newtonsoft.Json.dll"
  File "/oname=License.txt" ".\License.txt"
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

  Delete "$INSTDIR\Uninstall.exe"
  Delete "$INSTDIR\BrowserSelect.exe"
  Delete "$INSTDIR\BrowserSelect.exe.config"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\License.txt"
  Delete "$SMPROGRAMS\BrowserSelect.lnk"

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
