<#
.SYNOPSIS
  Opens a URL (or file) with BrowserSelect for manual testing.

.DESCRIPTION
  Starts BrowserSelect.exe directly with the given URL, the same way Windows does when a
  link is clicked and BrowserSelect is the default browser. Useful for the human test
  recipes in Tests\human_test (no need to change the default browser for most tests).

  Because the script starts BrowserSelect itself, "Source App" rules will see
  powershell.exe (Windows PowerShell) or pwsh.exe (PowerShell 7) as the source application.

.PARAMETER Url
  The URL or file path to open, e.g. https://example.com/docs/intro

.PARAMETER Delay
  Seconds to wait before launching (e.g. to hold the Alt key down for the Alt test).

.PARAMETER Maximized
  Launch BrowserSelect with a "maximized" show state (reproduces the old maximize bug).

.PARAMETER ViaShell
  Open the URL through the Windows default handler (Start-Process <url>) instead of
  calling BrowserSelect.exe directly. Requires BrowserSelect to be the default browser.

.PARAMETER Exe
  Full path of BrowserSelect.exe (auto-detected from the installer location if omitted).

.EXAMPLE
  powershell -ExecutionPolicy Bypass -File .\bs-open.ps1 https://example.com/
.EXAMPLE
  powershell -ExecutionPolicy Bypass -File .\bs-open.ps1 https://example.com/ -Delay 3
.EXAMPLE
  powershell -ExecutionPolicy Bypass -File .\bs-open.ps1 https://example.com/ -Maximized
#>
param(
    [Parameter(Mandatory = $true, Position = 0)][string]$Url,
    [int]$Delay = 0,
    [switch]$Maximized,
    [switch]$ViaShell,
    [string]$Exe
)

$ErrorActionPreference = 'Stop'

if (-not $ViaShell -and -not $Exe) {
    $candidates = @()
    $installDir = $null
    try {
        $installDir = (Get-ItemProperty -Path 'HKCU:\Software\BrowserSelect' -ErrorAction Stop).'(default)'
    } catch { }
    if ($installDir) { $candidates += (Join-Path $installDir 'BrowserSelect.exe') }
    $candidates += (Join-Path $env:LOCALAPPDATA 'BrowserSelect\BrowserSelect.exe')
    $Exe = $candidates | Where-Object { $_ -and (Test-Path $_) } | Select-Object -First 1
    if (-not $Exe) {
        Write-Host 'BrowserSelect.exe not found. Install it from the release page or pass -Exe "C:\path\BrowserSelect.exe".' -ForegroundColor Red
        exit 1
    }
}

if ($Delay -gt 0) {
    for ($i = $Delay; $i -gt 0; $i--) {
        Write-Host ("Launching in {0} s ..." -f $i)
        Start-Sleep -Seconds 1
    }
}

$style = 'Normal'
if ($Maximized) { $style = 'Maximized' }

if ($ViaShell) {
    Write-Host "Opening via Windows default handler: $Url"
    Start-Process -FilePath $Url -WindowStyle $style
} else {
    Write-Host "Opening with $Exe : $Url  (window style: $style)"
    Start-Process -FilePath $Exe -ArgumentList ('"' + $Url + '"') -WindowStyle $style
}
