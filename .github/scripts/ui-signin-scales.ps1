# Runs ui-screenshots.ps1 inside Remote Desktop sessions that are SIGNED IN at each display scale
# (loopback RDP to this runner with a temporary local user; the scale comes from the .rdp file's
# desktopscalefactor). Unlike changing the scale at run time, Windows then uses the scale as the
# "system DPI" too, which is what a user with a 125-200 % screen normally has.
# Used by .github/workflows/ui-screenshots.yml on the throw-away Actions runner only.
param(
    [string]$BuildDir = "BrowserSelect\bin\x64\Release",
    [string]$OutDir = "screenshots-ci",
    [string]$Scales = "100,125,150,175,200",
    [string]$ScriptDir = $PSScriptRoot
)
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing
Add-Type -Path (Join-Path $ScriptDir "Display.cs")
[Disp]::MakeDpiAware() | Out-Null
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null
$OutDir = (Resolve-Path $OutDir).Path

function ConsoleShot([string]$name) {
    $b = [System.Windows.Forms.Screen]::PrimaryScreen.Bounds
    $bmp = New-Object System.Drawing.Bitmap $b.Width, $b.Height
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.CopyFromScreen($b.X, $b.Y, 0, 0, $bmp.Size)
    $bmp.Save((Join-Path $OutDir "console-$name.png"), [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
}
Add-Type -AssemblyName System.Windows.Forms

# console: 1920x1080 at 100 % so the Remote Desktop window fits
[Disp]::SetResolution(1920, 1080) | Out-Null
[Disp]::SetScale(100) | Out-Null

# shared folder for the app copy, scripts and results
$root = "C:\bs"
Remove-Item -Recurse -Force $root -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path "$root\app", "$root\out", "$root\layout" | Out-Null
Copy-Item -Recurse -Force (Join-Path $BuildDir "*") "$root\app"
Copy-Item -Force (Join-Path $ScriptDir "ui-screenshots.ps1"), (Join-Path $ScriptDir "Display.cs") $root
icacls $root /grant "Everyone:(OI)(CI)F" /T /Q | Out-Null

# runs at every sign-in of the test user, inside its session
@'
if ($env:USERNAME -ne "bstest") { return }
$scale = (Get-Content C:\bs\job.txt).Trim()
Start-Transcript -Path "C:\bs\out\session-$scale.log" -Force | Out-Null
try {
    Start-Sleep -Seconds 15  # first sign-in: let the shell settle
    & C:\bs\ui-screenshots.ps1 -Mode session -BuildDir C:\bs\app -OutDir "C:\bs\out\s$scale" -DisplayCs C:\bs\Display.cs -LayoutDir "C:\bs\layout\$scale"
} catch { Write-Host "ERROR: $_" }
Stop-Transcript | Out-Null
Set-Content "C:\bs\done-$scale.txt" "done"
'@ | Set-Content -Encoding UTF8 "$root\in-session.ps1"
$pwsh = (Get-Command pwsh).Source
New-ItemProperty -Force -Path "HKLM:\Software\Microsoft\Windows\CurrentVersion\Run" -Name "bs-ui-test" -Value "`"$pwsh`" -NoProfile -ExecutionPolicy Bypass -WindowStyle Minimized -File C:\bs\in-session.ps1" | Out-Null

# temporary user (non-admin, Remote Desktop Users) with a random password; Remote Desktop on
$pw = "Bs!" + [Guid]::NewGuid().ToString("N").Substring(0, 20) + "aZ9"
net user bstest $pw /add /y | Out-Null
net localgroup "Remote Desktop Users" bstest /add | Out-Null
Set-ItemProperty "HKLM:\System\CurrentControlSet\Control\Terminal Server" -Name fDenyTSConnections -Value 0
Start-Service TermService -ErrorAction SilentlyContinue
$target = "127.0.0.2"
cmdkey /generic:"TERMSRV/$target" /user:"$env:COMPUTERNAME\bstest" /pass:"$pw" | Out-Null

# screen size per scale: typical panels for that scale, big enough for every window
$sizes = @{ 100 = "1920x1080"; 125 = "1920x1080"; 150 = "2560x1440"; 175 = "2560x1600"; 200 = "3840x2160" }
$summary = New-Object System.Collections.Generic.List[string]
foreach ($scale in ($Scales -split ',' | ForEach-Object { [int]$_ })) {
    $wh = $sizes[$scale]; if (-not $wh) { $wh = "2560x1600" }
    $w, $h = $wh -split 'x'
    Set-Content "$root\job.txt" "$scale"
    @(
        "full address:s:$target", "username:s:$env:COMPUTERNAME\bstest", "screen mode id:i:1",
        "desktopwidth:i:$w", "desktopheight:i:$h", "session bpp:i:32", "smart sizing:i:1",
        "desktopscalefactor:i:$scale", "devicescalefactor:i:100", "dynamic resolution:i:0",
        "authentication level:i:0", "prompt for credentials:i:0", "enablecredsspsupport:i:1",
        "redirectclipboard:i:0", "redirectprinters:i:0", "redirectdrives:i:0", "redirectsmartcards:i:0",
        "audiomode:i:2", "autoreconnection enabled:i:0"
    ) | Set-Content -Encoding ASCII "$root\s.rdp"
    Write-Host "=== signing in at $scale % ($wh)"
    $m = Start-Process mstsc -ArgumentList "$root\s.rdp" -PassThru
    $done = "$root\done-$scale.txt"
    $sw = [Diagnostics.Stopwatch]::StartNew()
    while (-not (Test-Path $done) -and $sw.Elapsed.TotalMinutes -lt 8) {
        Start-Sleep -Seconds 10
        if ([int]$sw.Elapsed.TotalSeconds % 60 -lt 10) { Write-Host "  waiting ($([int]$sw.Elapsed.TotalSeconds) s) $((quser 2>$null) -join ' | ')" }
        if ($sw.Elapsed.TotalSeconds -gt 45 -and $sw.Elapsed.TotalSeconds -lt 56) { ConsoleShot "$scale-after-45s" }
    }
    if (Test-Path $done) { Write-Host "  finished in $([int]$sw.Elapsed.TotalSeconds) s" }
    else { Write-Host "::warning::$scale %: no result after 8 minutes"; ConsoleShot "$scale-timeout" }
    if (Test-Path "$root\out\session-$scale.log") { Get-Content "$root\out\session-$scale.log" | Select-String -NotMatch "^\*{5}|^(Username|RunAs|Configuration|Machine|Host Application|Process ID|PSVersion|PSEdition|PSCompatibleVersions|BuildVersion|CLRVersion|WSManStackVersion|PSRemotingProtocolVersion|SerializationVersion|Start time|End time|Windows PowerShell transcript|PowerShell transcript)" | ForEach-Object { Write-Host "  | $_" } }
    # sign the test user out before the next scale
    $line = (quser 2>$null) | Where-Object { $_ -match "bstest" }
    if ($line) { $id = ($line -split '\s+' | Where-Object { $_ -match '^\d+$' } | Select-Object -First 1); if ($id) { logoff $id } }
    Start-Sleep -Seconds 5
    if (-not $m.HasExited) { $m.Kill() }
    Start-Sleep -Seconds 5
}
Remove-ItemProperty -Path "HKLM:\Software\Microsoft\Windows\CurrentVersion\Run" -Name "bs-ui-test" -ErrorAction SilentlyContinue
cmdkey /delete:"TERMSRV/$target" | Out-Null

# results: <OutDir>/<scale>/..., one summary
Get-ChildItem "$root\out" -Directory | ForEach-Object {
    Get-ChildItem $_.FullName | Where-Object { $_.PSIsContainer } | ForEach-Object { Copy-Item -Recurse -Force $_.FullName $OutDir }
    if (Test-Path "$($_.FullName)\summary.md") { Get-Content "$($_.FullName)\summary.md" | Add-Content -Encoding utf8 (Join-Path $OutDir "summary.md") }
}
Copy-Item -Force "$root\out\session-*.log" $OutDir -ErrorAction SilentlyContinue
if ($env:GITHUB_STEP_SUMMARY -and (Test-Path (Join-Path $OutDir "summary.md"))) { Get-Content (Join-Path $OutDir "summary.md") | Add-Content $env:GITHUB_STEP_SUMMARY }
