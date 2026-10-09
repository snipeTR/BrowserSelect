# Takes screenshots of every BrowserSelect window (browser list, Settings, Edit browser, About,
# Original project info, ? help) in the Light and Dark themes, English and Turkish, at several
# display scales (used by .github/workflows/ui-screenshots.yml; run manually, never creates releases).
#
# Two ways to get a display scale on the runner:
#  -Mode runtime : the scale of the runner's monitor is changed at run time the same way Settings >
#                  Display > Scale does (DisplayConfigSetDeviceInfo, see Display.cs). Windows keeps the
#                  sign-in DPI (96) as "system DPI", so this is the same situation as a second monitor
#                  with a different scale than the main one.
#  -Mode session : just uses the scale the current session was signed in with (manual use on a PC;
#                  a loopback Remote Desktop sign-in on the Actions runner never logged in).
# The DPI of every window, its pixel size and the app's own layout report are recorded and checked.
param(
    [string]$BuildDir = "BrowserSelect\bin\x64\Release",
    [string]$OutDir = "screenshots-ci",
    [ValidateSet("runtime", "session")] [string]$Mode = "runtime",
    [string]$Scales = "100,125,150,175,200",
    [string]$DisplayCs = "$PSScriptRoot\Display.cs",
    [string]$LayoutDir = ""
)
$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName UIAutomationClient
Add-Type -AssemblyName UIAutomationTypes
Add-Type -Path $DisplayCs
Write-Host "$([Disp]::MakeDpiAware()) system dpi $([Disp]::SystemDpi()) monitor dpi $([Disp]::MonitorDpi()) resolution $([Disp]::Resolution()) user $env:USERNAME" 
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
  [DllImport("user32.dll")] public static extern bool PostMessage(IntPtr h, uint m, IntPtr w, IntPtr l);
  [DllImport("user32.dll")] public static extern bool IsWindow(IntPtr h);
  [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint x, uint y, uint d, IntPtr e);
  public static void Click(int x, int y) { SetCursorPos(x, y); System.Threading.Thread.Sleep(200); mouse_event(2,0,0,0,IntPtr.Zero); mouse_event(4,0,0,0,IntPtr.Zero); }
  [DllImport("user32.dll")] public static extern void keybd_event(byte k, byte s, uint f, IntPtr e);
  public static void Key(byte k) { keybd_event(k,0,0,IntPtr.Zero); keybd_event(k,0,2,IntPtr.Zero); }
  public static void Close(IntPtr h) { PostMessage(h, 0x0010, IntPtr.Zero, IntPtr.Zero); }
}
"@

New-Item -ItemType Directory -Force -Path $OutDir | Out-Null
$layoutDir = if ($LayoutDir) { $LayoutDir } else { Join-Path $env:TEMP "bs-layout" }
$env:BROWSERSELECT_LAYOUT_LOG = $layoutDir
$summary = New-Object System.Collections.Generic.List[string]
$problems = New-Object System.Collections.Generic.List[string]

function Shot([IntPtr]$h, [string]$name, [string]$formName) {
    $r = New-Object W+RECT
    [W]::GetWindowRect($h, [ref]$r) | Out-Null
    $w = $r.R - $r.L; $hgt = $r.B - $r.T
    if ($w -le 0 -or $hgt -le 0) { Write-Host "::warning::window for $name has no size"; return }
    $bmp = New-Object System.Drawing.Bitmap $w, $hgt
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.CopyFromScreen($r.L, $r.T, 0, 0, $bmp.Size)
    $bmp.Save((Join-Path $script:dir "$name.png"), [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
    $dpi = [Disp]::WindowDpi($h)
    Write-Host "saved $script:scale/$name.png ($w x $hgt, window dpi $dpi)"
    $script:summary.Add("| $script:scale | $name | $w x $hgt | $dpi | $script:systemDpi |")
    # monitor DPI (drawn at the scale) or the sign-in DPI (Windows stretches a system DPI aware window)
    if ($dpi -ne $script:expectedDpi -and $dpi -ne $script:systemDpi) { $script:problems.Add("$script:scale/${name}: window DPI $dpi, expected $script:expectedDpi") }
    # the app's own layout report (BROWSERSELECT_LAYOUT_LOG), written ~0.7 s after the window is shown
    $report = Join-Path $layoutDir "$formName.txt"
    for ($i = 0; $i -lt 20 -and -not (Test-Path $report); $i++) { Start-Sleep -Milliseconds 250 }
    if (Test-Path $report) {
        Move-Item -Force $report (Join-Path $script:dir "$name.layout.txt")
        $script:layouts[[string]$h] = @{ File = (Join-Path $script:dir "$name.layout.txt"); L = $r.L; T = $r.T; W = $w }
        $first = Get-Content (Join-Path $script:dir "$name.layout.txt") -TotalCount 1
        if ($first -ne "issues: 0") {
            foreach ($l in (Get-Content (Join-Path $script:dir "$name.layout.txt") | Select-Object -Skip 1)) {
                if ($l -eq "") { break }
                $script:problems.Add("$script:scale/${name}: $($l.Trim())")
            }
        }
    } else { $script:problems.Add("$script:scale/${name}: no layout report") }
    # control bounds as seen by UI Automation (window coordinates, physical pixels)
    try {
        $root = [System.Windows.Automation.AutomationElement]::FromHandle($h)
        $all = $root.FindAll([System.Windows.Automation.TreeScope]::Descendants, [System.Windows.Automation.Condition]::TrueCondition)
        $lines = foreach ($el in $all) {
            $b = $el.Current.BoundingRectangle
            "{0,-12} {1,5},{2,5} {3,4}x{4,-4} {5} [{6}]" -f $el.Current.ControlType.ProgrammaticName.Replace("ControlType.", ""), ($b.X - $r.L), ($b.Y - $r.T), $b.Width, $b.Height, $el.Current.Name, $el.Current.AutomationId
        }
        $lines | Out-File -Encoding utf8 (Join-Path $script:dir "$name.uia.txt")
    } catch { Write-Host "::warning::UI Automation dump failed for $name" }
}

function ClientPoint([IntPtr]$h, [int]$x, [int]$y) {
    $p = New-Object W+POINT
    $p.X = $x; $p.Y = $y
    [W]::ClientToScreen($h, [ref]$p) | Out-Null
    return $p
}

$script:layouts = @{}

# clicks a control of a BrowserSelect window by its (WinForms) name, using the positions from the
# app's layout report (window relative, scaled to physical pixels if Windows stretches the window)
function ClickCtl([IntPtr]$h, [string]$name, [int]$index = 0, [double]$fx = 0.5, [double]$fy = 0.5) {
    $info = $script:layouts[[string]$h]
    if ($info -eq $null) { throw "no layout report for window $h" }
    $lines = Get-Content $info.File
    $win = ($lines | Where-Object { $_ -like "#win *" } | Select-Object -First 1) -split ' '
    $ratio = $info.W / [double]$win[1]
    $found = New-Object System.Collections.Generic.List[object]
    foreach ($l in $lines) { if ($l -like "#ctl $name *") { $found.Add([string[]]($l -split ' ')) } }
    if ($found.Count -le $index) { throw "control $name not found" }
    $sorted = [object[]]($found.ToArray())
    [Array]::Sort($sorted, [Comparison[object]] { param($a, $b) ([int]$a[3]).CompareTo([int]$b[3]) })
    $c = $sorted[$index]
    if ($c[6] -ne "1" -and -not $script:forceEnabled) { throw "control $name is disabled" }
    $script:forceEnabled = $false
    $x = $info.L + ([int]$c[2] + [int]$c[4] * $fx) * $ratio
    $y = $info.T + ([int]$c[3] + [int]$c[5] * $fy) * $ratio
    [W]::Click([int]$x, [int]$y)
}

function CtlEnabled([IntPtr]$h, [string]$name) {
    $info = $script:layouts[[string]$h]
    $line = Get-Content $info.File | Where-Object { $_ -like "#ctl $name *" } | Select-Object -First 1
    return ($line -ne $null -and ($line -split ' ')[6] -eq "1")
}

function FindId([IntPtr]$h, [string]$id) {
    $root = [System.Windows.Automation.AutomationElement]::FromHandle($h)
    $cond = New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::AutomationIdProperty, $id)
    return $root.FindFirst([System.Windows.Automation.TreeScope]::Descendants, $cond)
}

function ClickElement($el) {
    $b = $el.Current.BoundingRectangle
    [W]::Click([int]($b.X + $b.Width / 2), [int]($b.Y + $b.Height / 2))
}

# waits for a new foreground window of BrowserSelect (a dialog opened by a click)
function NewWindow([IntPtr]$owner) {
    for ($i = 0; $i -lt 32; $i++) {
        Start-Sleep -Milliseconds 250
        $fg = [W]::GetForegroundWindow()
        if ($fg -ne [IntPtr]::Zero -and $fg -ne $owner -and [Disp]::OwnedVisible($fg, $script:p.Id)) { Start-Sleep -Milliseconds 1200; return $fg }
    }
    return [IntPtr]::Zero
}

# the vertical About/Settings buttons (ButtonsUC, created in code without names), top to bottom
function SideButtons([IntPtr]$main) {
    $uc = FindId $main "ButtonsUC"
    if ($uc -eq $null) { return @() }
    $cond = New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ControlTypeProperty, [System.Windows.Automation.ControlType]::Button)
    return @($uc.FindAll([System.Windows.Automation.TreeScope]::Descendants, $cond) | Sort-Object { $_.Current.BoundingRectangle.Y })
}

function CloseWindow([IntPtr]$h) {
    [W]::Close($h)
    for ($i = 0; $i -lt 20 -and [W]::IsWindow($h); $i++) { Start-Sleep -Milliseconds 150 }
    Start-Sleep -Milliseconds 400
}

$config = Join-Path $BuildDir "BrowserSelect.exe.config"
$original = Get-Content $config -Raw
$exe = (Resolve-Path (Join-Path $BuildDir "BrowserSelect.exe")).Path

function RunAll {
    foreach ($run in @(@("Light", "en"), @("Dark", "en"), @("Light", "tr"), @("Dark", "tr"))) {
        $theme = $run[0]; $lang = $run[1]
        # no user.config: the defaults in BrowserSelect.exe.config are used
        $xml = $original -replace '(<setting name="Theme" serializeAs="String">\s*<value>)[^<]*(</value>)', "`${1}$theme`${2}"
        $xml = $xml -replace '(<setting name="Language" serializeAs="String">\s*<value>)[^<]*(</value>)', "`${1}$lang`${2}"
        $tag = "$theme-$lang"
        Set-Content -Path $config -Value $xml -Encoding UTF8
        Remove-Item -Recurse -Force $layoutDir -ErrorAction SilentlyContinue

        $script:p = Start-Process -FilePath $exe -ArgumentList "https://example.com/?bs-test=screenshot" -PassThru
        $p = $script:p
        Start-Sleep -Seconds 6
        $p.Refresh()
        if ($p.HasExited) { throw "BrowserSelect exited ($tag, $script:scale %), exit code $($p.ExitCode)" }
        $main = $p.MainWindowHandle
        [W]::SetForegroundWindow($main) | Out-Null
        Start-Sleep -Milliseconds 500
        Shot $main "$tag-1-browser-list" "Form1"
        # Settings (2nd vertical button), then Edit browser from it
        try { ClickCtl $main "VButton" 1 } catch { $script:problems.Add("$script:scale/${tag}: $_"); $p.Kill(); continue }
        $settings = NewWindow $main
        if ($settings -ne [IntPtr]::Zero) {
            Shot $settings "$tag-2-settings" "frm_settings"
            try {
                # select the first browser of the list (click its text, top of the list), then Edit...
                ClickCtl $settings "browser_filter" 0 0.6 0.03
                Start-Sleep -Milliseconds 400
                [W]::Key(0x24)  # Home: first browser selected (enables Edit...)
                Start-Sleep -Milliseconds 800
                # the report was written before the selection; Edit... is enabled now
                $script:forceEnabled = $true
                ClickCtl $settings "btn_browser_edit"
                $edit = NewWindow $settings
                if ($edit -ne [IntPtr]::Zero) { Shot $edit "$tag-5-edit-browser" "frm_browser_edit"; CloseWindow $edit }
                else { $script:problems.Add("$script:scale/${tag}: Edit browser window did not open") }
            } catch { $script:problems.Add("$script:scale/${tag}: Edit browser: $_") }
            [W]::SetForegroundWindow($settings) | Out-Null
            CloseWindow $settings
        } else { $script:problems.Add("$script:scale/${tag}: Settings window did not open") }

        # About, then Original project info from it
        [W]::SetForegroundWindow($main) | Out-Null
        Start-Sleep -Milliseconds 500
        ClickCtl $main "VButton" 0
        $about = NewWindow $main
        if ($about -ne [IntPtr]::Zero) {
            Shot $about "$tag-3-about" "frm_About"
            try {
                ClickCtl $about "btn_original"
                $orig = NewWindow $about
                if ($orig -ne [IntPtr]::Zero) { Shot $orig "$tag-6-original-info" "frm_about_original"; CloseWindow $orig }
                else { $script:problems.Add("$script:scale/${tag}: Original project info window did not open") }
            } catch { $script:problems.Add("$script:scale/${tag}: Original project info: $_") }
            [W]::SetForegroundWindow($about) | Out-Null
            CloseWindow $about
        } else { $script:problems.Add("$script:scale/${tag}: About window did not open") }

        # ? (help) button at the bottom right
        [W]::SetForegroundWindow($main) | Out-Null
        Start-Sleep -Milliseconds 500
        ClickCtl $main "btn_help"
        $help = NewWindow $main
        if ($help -ne [IntPtr]::Zero) { Shot $help "$tag-4-help" "frm_help_main"; CloseWindow $help }
        else { $script:problems.Add("$script:scale/${tag}: Help window did not open") }

        if (-not $p.HasExited) { $p.Kill() }
        Start-Sleep -Seconds 1
    }
}

function Begin([string]$label) {
    $script:systemDpi = [Disp]::SystemDpi()
    $script:expectedDpi = [int][Disp]::MonitorDpi()
    $script:scale = $label
    $script:dir = Join-Path $OutDir $label
    New-Item -ItemType Directory -Force -Path $script:dir | Out-Null
}

if ($Mode -eq "session") {
    # signed in at the scale: nothing to change, just check that the session really has it
    $label = [string][int][Math]::Round([Disp]::MonitorDpi() * 100 / 96)
    Write-Host "=== session at $label % : system dpi $([Disp]::SystemDpi()), monitor dpi $([Disp]::MonitorDpi()), resolution $([Disp]::Resolution())"
    Begin $label
    RunAll
} else {
    foreach ($scale in ($Scales -split ',' | ForEach-Object { [int]$_ })) {
        # 1920x1080 allows up to 175 %; 200 % needs at least 1200 lines (1600x1200 is the biggest such mode here)
        $res = if ($scale -gt 175) { @(1600, 1200) } else { @(1920, 1080) }
        $c = [Disp]::SetResolution($res[0], $res[1])
        Start-Sleep -Seconds 2
        $c2 = [Disp]::SetScale($scale)
        Start-Sleep -Seconds 3
        $monitorDpi = [Disp]::MonitorDpi()
        Write-Host "=== scale $scale % : resolution $([Disp]::Resolution()) (set $c), scale set $c2, $([Disp]::ScaleInfo()), monitor dpi $monitorDpi, system dpi $([Disp]::SystemDpi())"
        if ($monitorDpi -ne [int][Math]::Round(96 * $scale / 100)) {
            Write-Host "::warning::scale $scale % could not be set (monitor dpi $monitorDpi), skipped"
            $summary.Add("| $scale | (not available: monitor dpi $monitorDpi) | | | |")
            continue
        }
        Begin "$scale"
        RunAll
    }
    [Disp]::SetScale(100) | Out-Null
}
Set-Content -Path $config -Value $original -Encoding UTF8

$md = @("# UI screenshots", "", "Mode: $Mode", "", "| scale % | window | size (px) | window DPI | system DPI |", "|---|---|---|---|---|") + $summary + @("", "## Problems ($($problems.Count))", "") + ($problems | ForEach-Object { "- $_" })
$md | Out-File -Encoding utf8 (Join-Path $OutDir "summary.md")
if ($env:GITHUB_STEP_SUMMARY) { $md | Out-File -Append -Encoding utf8 $env:GITHUB_STEP_SUMMARY }
Write-Host "problems: $($problems.Count)"
$problems | ForEach-Object { Write-Host "  $_" }
