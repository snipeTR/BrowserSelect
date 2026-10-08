# Takes screenshots of the browser list, Settings and About windows in the Light and Dark themes
# (used by .github/workflows/ui-screenshots.yml; run manually, never creates releases).
param(
    [string]$BuildDir = "BrowserSelect\bin\x64\Release",
    [string]$OutDir = "screenshots-ci"
)
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing
Add-Type @"
using System;
using System.Runtime.InteropServices;
public static class W {
  [StructLayout(LayoutKind.Sequential)] public struct RECT { public int L, T, R, B; }
  [StructLayout(LayoutKind.Sequential)] public struct POINT { public int X, Y; }
  [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr h, out RECT r);
  [DllImport("user32.dll")] public static extern bool GetClientRect(IntPtr h, out RECT r);
  [DllImport("user32.dll")] public static extern bool ClientToScreen(IntPtr h, ref POINT p);
  [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
  [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
  [DllImport("user32.dll")] public static extern bool SetCursorPos(int x, int y);
  [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint x, uint y, uint d, IntPtr e);
  [DllImport("user32.dll")] public static extern void keybd_event(byte k, byte s, uint f, IntPtr e);
  public static void Click(int x, int y) { SetCursorPos(x, y); System.Threading.Thread.Sleep(200); mouse_event(2,0,0,0,IntPtr.Zero); mouse_event(4,0,0,0,IntPtr.Zero); }
  public static void Esc() { keybd_event(0x1B,0,0,IntPtr.Zero); keybd_event(0x1B,0,2,IntPtr.Zero); }
}
"@

New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

function Shot([IntPtr]$h, [string]$name) {
    $r = New-Object W+RECT
    [W]::GetWindowRect($h, [ref]$r) | Out-Null
    $w = $r.R - $r.L; $hgt = $r.B - $r.T
    if ($w -le 0 -or $hgt -le 0) { Write-Host "::warning::window for $name has no size"; return }
    $bmp = New-Object System.Drawing.Bitmap $w, $hgt
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.CopyFromScreen($r.L, $r.T, 0, 0, $bmp.Size)
    $bmp.Save((Join-Path $OutDir "$name.png"), [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
    Write-Host "saved $name.png ($w x $hgt)"
}

function ClientPoint([IntPtr]$h, [int]$x, [int]$y) {
    $p = New-Object W+POINT
    $p.X = $x; $p.Y = $y
    [W]::ClientToScreen($h, [ref]$p) | Out-Null
    return $p
}

$config = Join-Path $BuildDir "BrowserSelect.exe.config"
$original = Get-Content $config -Raw
$exe = (Resolve-Path (Join-Path $BuildDir "BrowserSelect.exe")).Path

foreach ($theme in @("Light", "Dark")) {
    # no user.config on the runner: the defaults in BrowserSelect.exe.config are used
    $xml = $original -replace '(<setting name="Theme" serializeAs="String">\s*<value>)[^<]*(</value>)', "`${1}$theme`${2}"
    Set-Content -Path $config -Value $xml -Encoding UTF8

    $p = Start-Process -FilePath $exe -ArgumentList "https://example.com/?bs-test=screenshot" -PassThru
    Start-Sleep -Seconds 6
    $p.Refresh()
    if ($p.HasExited) { throw "BrowserSelect exited (theme $theme), exit code $($p.ExitCode)" }
    $main = $p.MainWindowHandle
    [W]::SetForegroundWindow($main) | Out-Null
    Start-Sleep -Milliseconds 500
    Shot $main "$theme-1-browser-list"

    # vertical buttons on the right: About (top) and Settings (below), see ButtonsUC.cs
    $cr = New-Object W+RECT
    [W]::GetClientRect($main, [ref]$cr) | Out-Null
    $rightClient = $cr.R + 8  # buttons are 20px wide, 5px from the left of the 25px wide ButtonsUC at the right edge
    $settings = ClientPoint $main ($rightClient - 20) 115
    [W]::Click($settings.X, $settings.Y)
    Start-Sleep -Seconds 3
    $fg = [W]::GetForegroundWindow()
    if ($fg -ne $main) { Shot $fg "$theme-2-settings"; [W]::Esc(); Start-Sleep -Seconds 1 }
    else { Write-Host "::warning::Settings window did not open ($theme)" }

    [W]::SetForegroundWindow($main) | Out-Null
    Start-Sleep -Milliseconds 500
    $about = ClientPoint $main ($rightClient - 20) 40
    [W]::Click($about.X, $about.Y)
    Start-Sleep -Seconds 3
    $fg = [W]::GetForegroundWindow()
    if ($fg -ne $main) { Shot $fg "$theme-3-about"; [W]::Esc(); Start-Sleep -Seconds 1 }
    else { Write-Host "::warning::About window did not open ($theme)" }

    if (-not $p.HasExited) { $p.Kill() }
    Start-Sleep -Seconds 1
}
Set-Content -Path $config -Value $original -Encoding UTF8
