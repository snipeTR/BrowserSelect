; BrowserSelect installer (Windows 64-bit)
;
; Build (from the BrowserSelect project folder, after a Release|x64 build):
;   makensis /DVERSION=1.4.2.0 installer.nsi
; Optional defines:
;   /DBUILD_DIR=<path to build output>   (default: .\bin\x64\Release)
;   /DOUTFILE=<installer file name>      (default: BrowserSelect-<VERSION>-x64-Setup.exe)
;
; Languages: a language selection dialog opens first (English, Turkish, Russian, German, French, Spanish,
; Portuguese (Brazil), Italian, Polish, Ukrainian, Chinese (Simplified), Japanese, Arabic, Persian). It preselects
; the Windows display language (English if that is not in the list), or the language chosen at the last install.
; The choice is stored in HKCU\Software\BrowserSelect "Installer Language" and reused by the uninstaller.
; Silent mode (/S) shows no dialog and uses the Windows display language.
; This only affects the setup/uninstall UI; the program's own languages are separate (English/Turkish).
; This file is saved as UTF-8 with BOM so makensis reads the translated texts correctly on any code page.
;
; .NET Framework 4.8 is required: setup checks it at start (registry, no admin rights needed). If it is missing,
; setup explains this, offers to open Microsoft's download page and quits (exit code 3 in silent mode).
; It never downloads or installs .NET Framework itself.
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

  ;remember the installer language (also used by the uninstaller)
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU"
  !define MUI_LANGDLL_REGISTRY_KEY "Software\BrowserSelect"
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"
  ;show the language dialog on every (non-silent) run, preselecting the remembered/system language
  !define MUI_LANGDLL_ALWAYSSHOW

  ;.NET Framework 4.8 = Release 528040 or higher (528049 on Windows 10 1903+ / Windows 11, 533320 for 4.8.1)
  !define NETFX48_MIN_RELEASE 528040
  !define NETFX48_DOWNLOAD_URL "https://dotnet.microsoft.com/download/dotnet-framework/net48"

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_LICENSE "./License.txt"
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES

  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages (installer UI only; the program itself is English/Turkish)
; The first MUI_LANGUAGE is the fallback when the Windows display language is not in this list.

  !insertmacro MUI_LANGUAGE "English"
  !insertmacro MUI_LANGUAGE "Turkish"
  !insertmacro MUI_LANGUAGE "Russian"
  !insertmacro MUI_LANGUAGE "German"
  !insertmacro MUI_LANGUAGE "French"
  !insertmacro MUI_LANGUAGE "Spanish"
  !insertmacro MUI_LANGUAGE "PortugueseBR"
  !insertmacro MUI_LANGUAGE "Italian"
  !insertmacro MUI_LANGUAGE "Polish"
  !insertmacro MUI_LANGUAGE "Ukrainian"
  !insertmacro MUI_LANGUAGE "SimpChinese"
  !insertmacro MUI_LANGUAGE "Japanese"
  !insertmacro MUI_LANGUAGE "Arabic"
  !insertmacro MUI_LANGUAGE "Farsi"

  ; keeps the LangDLL plugin at the start of the (solid) data block so the language dialog opens quickly
  !insertmacro MUI_RESERVEFILE_LANGDLL

  ; --- English
  LangString BS_Caption ${LANG_ENGLISH} "BrowserSelect ${VERSION} (64-bit) Installation"
  LangString BS_Requires64 ${LANG_ENGLISH} "This build of BrowserSelect requires 64-bit Windows."
  LangString BS_AppRunning ${LANG_ENGLISH} "BrowserSelect is currently running.$\r$\n$\r$\nSetup has to close it to continue. Do you want to close BrowserSelect now?"
  LangString BS_CloseRequired ${LANG_ENGLISH} "Setup cannot continue while BrowserSelect is running.$\r$\n$\r$\nPlease close the program and restart the setup."
  LangString BS_UnAppRunning ${LANG_ENGLISH} "BrowserSelect is currently running.$\r$\n$\r$\nIt has to be closed before it can be uninstalled. Do you want to close BrowserSelect now?"
  LangString BS_UnCloseRequired ${LANG_ENGLISH} "BrowserSelect cannot be uninstalled while it is running.$\r$\n$\r$\nPlease close the program and start the uninstall again."
  LangString BS_CloseFailed ${LANG_ENGLISH} "BrowserSelect could not be closed.$\r$\n$\r$\nPlease close the program manually and restart the setup."
  LangString BS_UnCloseFailed ${LANG_ENGLISH} "BrowserSelect could not be closed.$\r$\n$\r$\nPlease close the program manually and start the uninstall again."
  LangString BS_NetFx48Missing ${LANG_ENGLISH} "BrowserSelect requires .NET Framework 4.8.$\r$\n$\r$\n.NET Framework 4.8 was not found on this computer, and BrowserSelect does not work without it. Please install it first, then run this setup again.$\r$\n$\r$\nDo you want to open the Microsoft download page now?"

  ; --- Turkish
  LangString BS_Caption ${LANG_TURKISH} "BrowserSelect ${VERSION} (64-bit) Kurulumu"
  LangString BS_Requires64 ${LANG_TURKISH} "BrowserSelect'in bu sürümü 64-bit Windows gerektirir."
  LangString BS_AppRunning ${LANG_TURKISH} "BrowserSelect şu anda çalışıyor.$\r$\n$\r$\nKurulumun devam edebilmesi için programın kapatılması gerekiyor. BrowserSelect şimdi kapatılsın mı?"
  LangString BS_CloseRequired ${LANG_TURKISH} "BrowserSelect çalışırken kurulum devam edemez.$\r$\n$\r$\nLütfen programı kapatıp kurulumu yeniden başlatın."
  LangString BS_UnAppRunning ${LANG_TURKISH} "BrowserSelect şu anda çalışıyor.$\r$\n$\r$\nKaldırılabilmesi için programın kapatılması gerekiyor. BrowserSelect şimdi kapatılsın mı?"
  LangString BS_UnCloseRequired ${LANG_TURKISH} "BrowserSelect çalışırken kaldırılamaz.$\r$\n$\r$\nLütfen programı kapatıp kaldırma işlemini yeniden başlatın."
  LangString BS_CloseFailed ${LANG_TURKISH} "BrowserSelect kapatılamadı.$\r$\n$\r$\nLütfen programı elle kapatıp kurulumu yeniden başlatın."
  LangString BS_UnCloseFailed ${LANG_TURKISH} "BrowserSelect kapatılamadı.$\r$\n$\r$\nLütfen programı elle kapatıp kaldırma işlemini yeniden başlatın."
  LangString BS_NetFx48Missing ${LANG_TURKISH} "BrowserSelect için .NET Framework 4.8 gerekli.$\r$\n$\r$\nBu bilgisayarda .NET Framework 4.8 bulunamadı ve BrowserSelect onsuz çalışmaz. Lütfen önce onu kurun, ardından bu kurulumu yeniden çalıştırın.$\r$\n$\r$\nMicrosoft indirme sayfası şimdi açılsın mı?"

  ; --- Russian
  LangString BS_Caption ${LANG_RUSSIAN} "Установка BrowserSelect ${VERSION} (64-бит)"
  LangString BS_Requires64 ${LANG_RUSSIAN} "Для этой версии BrowserSelect требуется 64-разрядная Windows."
  LangString BS_AppRunning ${LANG_RUSSIAN} "Программа BrowserSelect сейчас запущена.$\r$\n$\r$\nЧтобы продолжить, программа установки должна её закрыть. Закрыть BrowserSelect сейчас?"
  LangString BS_CloseRequired ${LANG_RUSSIAN} "Установка не может быть продолжена, пока запущен BrowserSelect.$\r$\n$\r$\nЗакройте программу и запустите установку заново."
  LangString BS_UnAppRunning ${LANG_RUSSIAN} "Программа BrowserSelect сейчас запущена.$\r$\n$\r$\nПеред удалением её необходимо закрыть. Закрыть BrowserSelect сейчас?"
  LangString BS_UnCloseRequired ${LANG_RUSSIAN} "BrowserSelect нельзя удалить, пока программа запущена.$\r$\n$\r$\nЗакройте программу и запустите удаление заново."
  LangString BS_CloseFailed ${LANG_RUSSIAN} "Не удалось закрыть BrowserSelect.$\r$\n$\r$\nЗакройте программу вручную и запустите установку заново."
  LangString BS_UnCloseFailed ${LANG_RUSSIAN} "Не удалось закрыть BrowserSelect.$\r$\n$\r$\nЗакройте программу вручную и запустите удаление заново."
  LangString BS_NetFx48Missing ${LANG_RUSSIAN} "Для работы BrowserSelect требуется .NET Framework 4.8.$\r$\n$\r$\n.NET Framework 4.8 не найден на этом компьютере, а без него BrowserSelect не работает. Сначала установите его, затем запустите эту установку снова.$\r$\n$\r$\nОткрыть страницу загрузки Microsoft сейчас?"

  ; --- German
  LangString BS_Caption ${LANG_GERMAN} "BrowserSelect ${VERSION} (64-Bit) Installation"
  LangString BS_Requires64 ${LANG_GERMAN} "Diese Version von BrowserSelect erfordert ein 64-Bit-Windows."
  LangString BS_AppRunning ${LANG_GERMAN} "BrowserSelect wird gerade ausgeführt.$\r$\n$\r$\nDas Setup muss das Programm beenden, um fortzufahren. Möchten Sie BrowserSelect jetzt beenden?"
  LangString BS_CloseRequired ${LANG_GERMAN} "Das Setup kann nicht fortgesetzt werden, solange BrowserSelect ausgeführt wird.$\r$\n$\r$\nBitte beenden Sie das Programm und starten Sie das Setup erneut."
  LangString BS_UnAppRunning ${LANG_GERMAN} "BrowserSelect wird gerade ausgeführt.$\r$\n$\r$\nDas Programm muss beendet werden, bevor es deinstalliert werden kann. Möchten Sie BrowserSelect jetzt beenden?"
  LangString BS_UnCloseRequired ${LANG_GERMAN} "BrowserSelect kann nicht deinstalliert werden, solange es ausgeführt wird.$\r$\n$\r$\nBitte beenden Sie das Programm und starten Sie die Deinstallation erneut."
  LangString BS_CloseFailed ${LANG_GERMAN} "BrowserSelect konnte nicht beendet werden.$\r$\n$\r$\nBitte beenden Sie das Programm manuell und starten Sie das Setup erneut."
  LangString BS_UnCloseFailed ${LANG_GERMAN} "BrowserSelect konnte nicht beendet werden.$\r$\n$\r$\nBitte beenden Sie das Programm manuell und starten Sie die Deinstallation erneut."
  LangString BS_NetFx48Missing ${LANG_GERMAN} "BrowserSelect benötigt .NET Framework 4.8.$\r$\n$\r$\n.NET Framework 4.8 wurde auf diesem Computer nicht gefunden, und ohne es funktioniert BrowserSelect nicht. Bitte installieren Sie es zuerst und führen Sie dieses Setup danach erneut aus.$\r$\n$\r$\nMöchten Sie jetzt die Microsoft-Downloadseite öffnen?"

  ; --- French
  LangString BS_Caption ${LANG_FRENCH} "Installation de BrowserSelect ${VERSION} (64 bits)"
  LangString BS_Requires64 ${LANG_FRENCH} "Cette version de BrowserSelect nécessite Windows 64 bits."
  LangString BS_AppRunning ${LANG_FRENCH} "BrowserSelect est en cours d'exécution.$\r$\n$\r$\nLe programme d'installation doit le fermer pour continuer. Voulez-vous fermer BrowserSelect maintenant ?"
  LangString BS_CloseRequired ${LANG_FRENCH} "L'installation ne peut pas continuer tant que BrowserSelect est en cours d'exécution.$\r$\n$\r$\nVeuillez fermer le programme et relancer l'installation."
  LangString BS_UnAppRunning ${LANG_FRENCH} "BrowserSelect est en cours d'exécution.$\r$\n$\r$\nIl doit être fermé avant de pouvoir être désinstallé. Voulez-vous fermer BrowserSelect maintenant ?"
  LangString BS_UnCloseRequired ${LANG_FRENCH} "BrowserSelect ne peut pas être désinstallé tant qu'il est en cours d'exécution.$\r$\n$\r$\nVeuillez fermer le programme et relancer la désinstallation."
  LangString BS_CloseFailed ${LANG_FRENCH} "Impossible de fermer BrowserSelect.$\r$\n$\r$\nVeuillez fermer le programme manuellement et relancer l'installation."
  LangString BS_UnCloseFailed ${LANG_FRENCH} "Impossible de fermer BrowserSelect.$\r$\n$\r$\nVeuillez fermer le programme manuellement et relancer la désinstallation."
  LangString BS_NetFx48Missing ${LANG_FRENCH} "BrowserSelect nécessite .NET Framework 4.8.$\r$\n$\r$\n.NET Framework 4.8 est introuvable sur cet ordinateur, et BrowserSelect ne fonctionne pas sans lui. Veuillez d'abord l'installer, puis relancer cette installation.$\r$\n$\r$\nVoulez-vous ouvrir la page de téléchargement de Microsoft maintenant ?"

  ; --- Spanish
  LangString BS_Caption ${LANG_SPANISH} "Instalación de BrowserSelect ${VERSION} (64 bits)"
  LangString BS_Requires64 ${LANG_SPANISH} "Esta versión de BrowserSelect requiere Windows de 64 bits."
  LangString BS_AppRunning ${LANG_SPANISH} "BrowserSelect se está ejecutando.$\r$\n$\r$\nEl programa de instalación debe cerrarlo para continuar. ¿Desea cerrar BrowserSelect ahora?"
  LangString BS_CloseRequired ${LANG_SPANISH} "La instalación no puede continuar mientras BrowserSelect se esté ejecutando.$\r$\n$\r$\nCierre el programa y vuelva a iniciar la instalación."
  LangString BS_UnAppRunning ${LANG_SPANISH} "BrowserSelect se está ejecutando.$\r$\n$\r$\nDebe cerrarse antes de poder desinstalarlo. ¿Desea cerrar BrowserSelect ahora?"
  LangString BS_UnCloseRequired ${LANG_SPANISH} "BrowserSelect no se puede desinstalar mientras se está ejecutando.$\r$\n$\r$\nCierre el programa y vuelva a iniciar la desinstalación."
  LangString BS_CloseFailed ${LANG_SPANISH} "No se pudo cerrar BrowserSelect.$\r$\n$\r$\nCierre el programa manualmente y vuelva a iniciar la instalación."
  LangString BS_UnCloseFailed ${LANG_SPANISH} "No se pudo cerrar BrowserSelect.$\r$\n$\r$\nCierre el programa manualmente y vuelva a iniciar la desinstalación."
  LangString BS_NetFx48Missing ${LANG_SPANISH} "BrowserSelect requiere .NET Framework 4.8.$\r$\n$\r$\nNo se encontró .NET Framework 4.8 en este equipo y BrowserSelect no funciona sin él. Instálelo primero y luego vuelva a ejecutar esta instalación.$\r$\n$\r$\n¿Desea abrir ahora la página de descarga de Microsoft?"

  ; --- PortugueseBR
  LangString BS_Caption ${LANG_PORTUGUESEBR} "Instalação do BrowserSelect ${VERSION} (64 bits)"
  LangString BS_Requires64 ${LANG_PORTUGUESEBR} "Esta versão do BrowserSelect requer o Windows de 64 bits."
  LangString BS_AppRunning ${LANG_PORTUGUESEBR} "O BrowserSelect está em execução.$\r$\n$\r$\nO instalador precisa fechá-lo para continuar. Deseja fechar o BrowserSelect agora?"
  LangString BS_CloseRequired ${LANG_PORTUGUESEBR} "A instalação não pode continuar enquanto o BrowserSelect estiver em execução.$\r$\n$\r$\nFeche o programa e inicie a instalação novamente."
  LangString BS_UnAppRunning ${LANG_PORTUGUESEBR} "O BrowserSelect está em execução.$\r$\n$\r$\nEle precisa ser fechado antes de ser desinstalado. Deseja fechar o BrowserSelect agora?"
  LangString BS_UnCloseRequired ${LANG_PORTUGUESEBR} "O BrowserSelect não pode ser desinstalado enquanto estiver em execução.$\r$\n$\r$\nFeche o programa e inicie a desinstalação novamente."
  LangString BS_CloseFailed ${LANG_PORTUGUESEBR} "Não foi possível fechar o BrowserSelect.$\r$\n$\r$\nFeche o programa manualmente e inicie a instalação novamente."
  LangString BS_UnCloseFailed ${LANG_PORTUGUESEBR} "Não foi possível fechar o BrowserSelect.$\r$\n$\r$\nFeche o programa manualmente e inicie a desinstalação novamente."
  LangString BS_NetFx48Missing ${LANG_PORTUGUESEBR} "O BrowserSelect requer o .NET Framework 4.8.$\r$\n$\r$\nO .NET Framework 4.8 não foi encontrado neste computador e o BrowserSelect não funciona sem ele. Instale-o primeiro e depois execute esta instalação novamente.$\r$\n$\r$\nDeseja abrir a página de download da Microsoft agora?"

  ; --- Italian
  LangString BS_Caption ${LANG_ITALIAN} "Installazione di BrowserSelect ${VERSION} (64 bit)"
  LangString BS_Requires64 ${LANG_ITALIAN} "Questa versione di BrowserSelect richiede Windows a 64 bit."
  LangString BS_AppRunning ${LANG_ITALIAN} "BrowserSelect è in esecuzione.$\r$\n$\r$\nPer continuare, il programma di installazione deve chiuderlo. Chiudere BrowserSelect ora?"
  LangString BS_CloseRequired ${LANG_ITALIAN} "L'installazione non può continuare mentre BrowserSelect è in esecuzione.$\r$\n$\r$\nChiudere il programma e riavviare l'installazione."
  LangString BS_UnAppRunning ${LANG_ITALIAN} "BrowserSelect è in esecuzione.$\r$\n$\r$\nDeve essere chiuso prima di poter essere disinstallato. Chiudere BrowserSelect ora?"
  LangString BS_UnCloseRequired ${LANG_ITALIAN} "BrowserSelect non può essere disinstallato mentre è in esecuzione.$\r$\n$\r$\nChiudere il programma e riavviare la disinstallazione."
  LangString BS_CloseFailed ${LANG_ITALIAN} "Impossibile chiudere BrowserSelect.$\r$\n$\r$\nChiudere il programma manualmente e riavviare l'installazione."
  LangString BS_UnCloseFailed ${LANG_ITALIAN} "Impossibile chiudere BrowserSelect.$\r$\n$\r$\nChiudere il programma manualmente e riavviare la disinstallazione."
  LangString BS_NetFx48Missing ${LANG_ITALIAN} "BrowserSelect richiede .NET Framework 4.8.$\r$\n$\r$\n.NET Framework 4.8 non è stato trovato su questo computer e BrowserSelect non funziona senza di esso. Installarlo prima e poi eseguire di nuovo questa installazione.$\r$\n$\r$\nAprire ora la pagina di download di Microsoft?"

  ; --- Polish
  LangString BS_Caption ${LANG_POLISH} "Instalacja BrowserSelect ${VERSION} (64-bit)"
  LangString BS_Requires64 ${LANG_POLISH} "Ta wersja BrowserSelect wymaga 64-bitowego systemu Windows."
  LangString BS_AppRunning ${LANG_POLISH} "BrowserSelect jest obecnie uruchomiony.$\r$\n$\r$\nAby kontynuować, instalator musi go zamknąć. Czy zamknąć BrowserSelect teraz?"
  LangString BS_CloseRequired ${LANG_POLISH} "Instalacja nie może być kontynuowana, gdy BrowserSelect jest uruchomiony.$\r$\n$\r$\nZamknij program i uruchom instalację ponownie."
  LangString BS_UnAppRunning ${LANG_POLISH} "BrowserSelect jest obecnie uruchomiony.$\r$\n$\r$\nMusi zostać zamknięty przed odinstalowaniem. Czy zamknąć BrowserSelect teraz?"
  LangString BS_UnCloseRequired ${LANG_POLISH} "Nie można odinstalować BrowserSelect, gdy jest uruchomiony.$\r$\n$\r$\nZamknij program i uruchom dezinstalację ponownie."
  LangString BS_CloseFailed ${LANG_POLISH} "Nie udało się zamknąć BrowserSelect.$\r$\n$\r$\nZamknij program ręcznie i uruchom instalację ponownie."
  LangString BS_UnCloseFailed ${LANG_POLISH} "Nie udało się zamknąć BrowserSelect.$\r$\n$\r$\nZamknij program ręcznie i uruchom dezinstalację ponownie."
  LangString BS_NetFx48Missing ${LANG_POLISH} "BrowserSelect wymaga .NET Framework 4.8.$\r$\n$\r$\nNie znaleziono .NET Framework 4.8 na tym komputerze, a bez niego BrowserSelect nie działa. Najpierw go zainstaluj, a następnie ponownie uruchom tę instalację.$\r$\n$\r$\nCzy otworzyć teraz stronę pobierania Microsoft?"

  ; --- Ukrainian
  LangString BS_Caption ${LANG_UKRAINIAN} "Встановлення BrowserSelect ${VERSION} (64-біт)"
  LangString BS_Requires64 ${LANG_UKRAINIAN} "Для цієї версії BrowserSelect потрібна 64-розрядна Windows."
  LangString BS_AppRunning ${LANG_UKRAINIAN} "Програма BrowserSelect зараз запущена.$\r$\n$\r$\nЩоб продовжити, інсталятор має її закрити. Закрити BrowserSelect зараз?"
  LangString BS_CloseRequired ${LANG_UKRAINIAN} "Встановлення не може продовжитися, поки запущено BrowserSelect.$\r$\n$\r$\nЗакрийте програму та запустіть встановлення знову."
  LangString BS_UnAppRunning ${LANG_UKRAINIAN} "Програма BrowserSelect зараз запущена.$\r$\n$\r$\nПеред видаленням її потрібно закрити. Закрити BrowserSelect зараз?"
  LangString BS_UnCloseRequired ${LANG_UKRAINIAN} "BrowserSelect не можна видалити, поки програма запущена.$\r$\n$\r$\nЗакрийте програму та запустіть видалення знову."
  LangString BS_CloseFailed ${LANG_UKRAINIAN} "Не вдалося закрити BrowserSelect.$\r$\n$\r$\nЗакрийте програму вручну та запустіть встановлення знову."
  LangString BS_UnCloseFailed ${LANG_UKRAINIAN} "Не вдалося закрити BrowserSelect.$\r$\n$\r$\nЗакрийте програму вручну та запустіть видалення знову."
  LangString BS_NetFx48Missing ${LANG_UKRAINIAN} "Для роботи BrowserSelect потрібен .NET Framework 4.8.$\r$\n$\r$\n.NET Framework 4.8 не знайдено на цьому комп'ютері, а без нього BrowserSelect не працює. Спочатку встановіть його, а потім знову запустіть це встановлення.$\r$\n$\r$\nВідкрити сторінку завантаження Microsoft зараз?"

  ; --- SimpChinese
  LangString BS_Caption ${LANG_SIMPCHINESE} "BrowserSelect ${VERSION}（64 位）安装"
  LangString BS_Requires64 ${LANG_SIMPCHINESE} "此版本的 BrowserSelect 需要 64 位 Windows。"
  LangString BS_AppRunning ${LANG_SIMPCHINESE} "BrowserSelect 正在运行。$\r$\n$\r$\n安装程序需要关闭它才能继续。是否立即关闭 BrowserSelect？"
  LangString BS_CloseRequired ${LANG_SIMPCHINESE} "BrowserSelect 正在运行，安装无法继续。$\r$\n$\r$\n请关闭该程序，然后重新运行安装程序。"
  LangString BS_UnAppRunning ${LANG_SIMPCHINESE} "BrowserSelect 正在运行。$\r$\n$\r$\n必须先关闭它才能卸载。是否立即关闭 BrowserSelect？"
  LangString BS_UnCloseRequired ${LANG_SIMPCHINESE} "BrowserSelect 正在运行，无法卸载。$\r$\n$\r$\n请关闭该程序，然后重新开始卸载。"
  LangString BS_CloseFailed ${LANG_SIMPCHINESE} "无法关闭 BrowserSelect。$\r$\n$\r$\n请手动关闭该程序，然后重新运行安装程序。"
  LangString BS_UnCloseFailed ${LANG_SIMPCHINESE} "无法关闭 BrowserSelect。$\r$\n$\r$\n请手动关闭该程序，然后重新开始卸载。"
  LangString BS_NetFx48Missing ${LANG_SIMPCHINESE} "BrowserSelect 需要 .NET Framework 4.8。$\r$\n$\r$\n此计算机上未找到 .NET Framework 4.8，没有它 BrowserSelect 无法运行。请先安装它，然后重新运行此安装程序。$\r$\n$\r$\n是否立即打开 Microsoft 下载页面？"

  ; --- Japanese
  LangString BS_Caption ${LANG_JAPANESE} "BrowserSelect ${VERSION} (64 ビット) セットアップ"
  LangString BS_Requires64 ${LANG_JAPANESE} "このバージョンの BrowserSelect には 64 ビット版 Windows が必要です。"
  LangString BS_AppRunning ${LANG_JAPANESE} "BrowserSelect は現在実行中です。$\r$\n$\r$\n続行するには、セットアップで BrowserSelect を終了する必要があります。今すぐ BrowserSelect を終了しますか?"
  LangString BS_CloseRequired ${LANG_JAPANESE} "BrowserSelect の実行中はセットアップを続行できません。$\r$\n$\r$\nプログラムを終了してから、セットアップをもう一度実行してください。"
  LangString BS_UnAppRunning ${LANG_JAPANESE} "BrowserSelect は現在実行中です。$\r$\n$\r$\nアンインストールするには、BrowserSelect を終了する必要があります。今すぐ BrowserSelect を終了しますか?"
  LangString BS_UnCloseRequired ${LANG_JAPANESE} "BrowserSelect の実行中はアンインストールできません。$\r$\n$\r$\nプログラムを終了してから、アンインストールをもう一度実行してください。"
  LangString BS_CloseFailed ${LANG_JAPANESE} "BrowserSelect を終了できませんでした。$\r$\n$\r$\nプログラムを手動で終了してから、セットアップをもう一度実行してください。"
  LangString BS_UnCloseFailed ${LANG_JAPANESE} "BrowserSelect を終了できませんでした。$\r$\n$\r$\nプログラムを手動で終了してから、アンインストールをもう一度実行してください。"
  LangString BS_NetFx48Missing ${LANG_JAPANESE} "BrowserSelect には .NET Framework 4.8 が必要です。$\r$\n$\r$\nこのコンピューターに .NET Framework 4.8 が見つかりません。これがないと BrowserSelect は動作しません。先にインストールしてから、このセットアップをもう一度実行してください。$\r$\n$\r$\n今すぐ Microsoft のダウンロード ページを開きますか?"

  ; --- Arabic
  LangString BS_Caption ${LANG_ARABIC} "تثبيت BrowserSelect ${VERSION} (64 بت)"
  LangString BS_Requires64 ${LANG_ARABIC} "يتطلب هذا الإصدار من BrowserSelect نظام Windows بإصدار 64 بت."
  LangString BS_AppRunning ${LANG_ARABIC} "BrowserSelect قيد التشغيل حاليًا.$\r$\n$\r$\nيجب على برنامج الإعداد إغلاقه للمتابعة. هل تريد إغلاق BrowserSelect الآن؟"
  LangString BS_CloseRequired ${LANG_ARABIC} "لا يمكن متابعة الإعداد أثناء تشغيل BrowserSelect.$\r$\n$\r$\nيرجى إغلاق البرنامج ثم إعادة تشغيل الإعداد."
  LangString BS_UnAppRunning ${LANG_ARABIC} "BrowserSelect قيد التشغيل حاليًا.$\r$\n$\r$\nيجب إغلاقه قبل إلغاء تثبيته. هل تريد إغلاق BrowserSelect الآن؟"
  LangString BS_UnCloseRequired ${LANG_ARABIC} "لا يمكن إلغاء تثبيت BrowserSelect أثناء تشغيله.$\r$\n$\r$\nيرجى إغلاق البرنامج ثم بدء إلغاء التثبيت مرة أخرى."
  LangString BS_CloseFailed ${LANG_ARABIC} "تعذّر إغلاق BrowserSelect.$\r$\n$\r$\nيرجى إغلاق البرنامج يدويًا ثم إعادة تشغيل الإعداد."
  LangString BS_UnCloseFailed ${LANG_ARABIC} "تعذّر إغلاق BrowserSelect.$\r$\n$\r$\nيرجى إغلاق البرنامج يدويًا ثم بدء إلغاء التثبيت مرة أخرى."
  LangString BS_NetFx48Missing ${LANG_ARABIC} "يتطلب BrowserSelect وجود .NET Framework 4.8.$\r$\n$\r$\nلم يتم العثور على .NET Framework 4.8 على هذا الكمبيوتر، ولا يعمل BrowserSelect بدونه. يرجى تثبيته أولاً ثم تشغيل هذا الإعداد مرة أخرى.$\r$\n$\r$\nهل تريد فتح صفحة التنزيل من Microsoft الآن؟"

  ; --- Farsi
  LangString BS_Caption ${LANG_FARSI} "نصب BrowserSelect ${VERSION} (64 بیتی)"
  LangString BS_Requires64 ${LANG_FARSI} "این نسخه از BrowserSelect به ویندوز 64 بیتی نیاز دارد."
  LangString BS_AppRunning ${LANG_FARSI} "BrowserSelect در حال اجراست.$\r$\n$\r$\nبرای ادامه، برنامه نصب باید آن را ببندد. آیا می‌خواهید BrowserSelect اکنون بسته شود؟"
  LangString BS_CloseRequired ${LANG_FARSI} "تا زمانی که BrowserSelect در حال اجراست، نصب نمی‌تواند ادامه یابد.$\r$\n$\r$\nلطفاً برنامه را ببندید و نصب را دوباره اجرا کنید."
  LangString BS_UnAppRunning ${LANG_FARSI} "BrowserSelect در حال اجراست.$\r$\n$\r$\nپیش از حذف باید بسته شود. آیا می‌خواهید BrowserSelect اکنون بسته شود؟"
  LangString BS_UnCloseRequired ${LANG_FARSI} "تا زمانی که BrowserSelect در حال اجراست، نمی‌توان آن را حذف کرد.$\r$\n$\r$\nلطفاً برنامه را ببندید و حذف را دوباره آغاز کنید."
  LangString BS_CloseFailed ${LANG_FARSI} "بستن BrowserSelect ممکن نشد.$\r$\n$\r$\nلطفاً برنامه را به‌صورت دستی ببندید و نصب را دوباره اجرا کنید."
  LangString BS_UnCloseFailed ${LANG_FARSI} "بستن BrowserSelect ممکن نشد.$\r$\n$\r$\nلطفاً برنامه را به‌صورت دستی ببندید و حذف را دوباره آغاز کنید."
  LangString BS_NetFx48Missing ${LANG_FARSI} "BrowserSelect به .NET Framework 4.8 نیاز دارد.$\r$\n$\r$\n.NET Framework 4.8 روی این رایانه پیدا نشد و BrowserSelect بدون آن کار نمی‌کند. لطفاً ابتدا آن را نصب کنید و سپس این برنامه نصب را دوباره اجرا کنید.$\r$\n$\r$\nآیا می‌خواهید صفحه دانلود مایکروسافت اکنون باز شود؟"

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
;.NET Framework 4.8 check

; pushes 1 if .NET Framework 4.8 (or later 4.x) is installed, else 0. Reads only (no admin rights needed).
Function BSHasNetFx48
  Push $0
  Push $1
  StrCpy $1 0
  ; 64-bit registry view first, then the 32-bit (WOW6432Node) view as a fallback
  SetRegView 64
  ClearErrors
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
  ${IfNot} ${Errors}
  ${AndIf} $0 != ""
    ${If} $0 >= ${NETFX48_MIN_RELEASE}
      StrCpy $1 1
    ${EndIf}
  ${EndIf}
  ${If} $1 == 0
    SetRegView 32
    ClearErrors
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
    ${IfNot} ${Errors}
    ${AndIf} $0 != ""
      ${If} $0 >= ${NETFX48_MIN_RELEASE}
        StrCpy $1 1
      ${EndIf}
    ${EndIf}
  ${EndIf}
  SetRegView 64
  StrCpy $0 $1
  Pop $1
  Exch $0
FunctionEnd

;--------------------------------
;Startup checks: language, 64-bit only, .NET Framework 4.8

Function .onInit
  ; language selection dialog (skipped in silent mode)
  !insertmacro MUI_LANGDLL_DISPLAY

  ${IfNot} ${RunningX64}
    MessageBox MB_OK|MB_ICONSTOP "$(BS_Requires64)" /SD IDOK
    SetErrorLevel 2
    Abort
  ${EndIf}

  ; BrowserSelect does not start without .NET Framework 4.8: explain, offer the download page, quit.
  ; Never downloads or installs it (that would need admin rights).
  Push $0
  Call BSHasNetFx48
  Pop $0
  ${If} $0 != 1
    ${IfNot} ${Silent}
      MessageBox MB_YESNO|MB_ICONEXCLAMATION|MB_DEFBUTTON1 "$(BS_NetFx48Missing)" IDNO +2
        ExecShell "open" "${NETFX48_DOWNLOAD_URL}"
    ${EndIf}
    SetErrorLevel 3
    Abort
  ${EndIf}
  Pop $0

  SetRegView 64
  ; BrowserSelect must not be running while its files are replaced
  !insertmacro BS_CLOSE_RUNNING "" BS_AppRunning BS_CloseRequired BS_CloseFailed
FunctionEnd

Function un.onInit
  ; language chosen at install time (HKCU\Software\BrowserSelect "Installer Language")
  !insertmacro MUI_UNGETLANGUAGE
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
  ;installer language for the uninstaller (MUI also saves it after a non-silent install; this covers /S)
  WriteRegStr HKCU "Software\BrowserSelect" "Installer Language" $LANGUAGE
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

  DeleteRegValue HKCU "Software\BrowserSelect" "Installer Language"
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
