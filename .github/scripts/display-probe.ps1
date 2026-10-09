Add-Type -Path "$PSScriptRoot\Display.cs"
Write-Host ([Disp]::MakeDpiAware())
Write-Host "resolution: $([Disp]::Resolution())"
Write-Host "modes: $([Disp]::Modes())"
foreach ($r in @(@(2560,1600), @(2560,1440), @(1920,1200), @(1920,1080))) {
  $c = [Disp]::SetResolution($r[0], $r[1]); Write-Host "set $($r[0])x$($r[1]) -> $c ; now $([Disp]::Resolution())"
  if ($c -eq 0) { break }
}
Start-Sleep 2
Write-Host "scale: $([Disp]::ScaleInfo()) monitorDpi=$([Disp]::MonitorDpi())"
foreach ($s in 125,150,175,200,100) {
  $c = [Disp]::SetScale($s); Start-Sleep 2
  Write-Host "set $s -> $c ; $([Disp]::ScaleInfo()) monitorDpi=$([Disp]::MonitorDpi())"
}
