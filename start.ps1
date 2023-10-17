$scriptDirectory = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition
Set-Location -Path $scriptDirectory
$shortcutFilePath = "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp\rest.lnk"
$targetPath ="$scriptDirectory"+ "\" + "rest.exe"
$shell = New-Object -ComObject WScript.Shell
$shortcut = $shell.CreateShortcut($shortcutFilePath)
$shortcut.TargetPath = $targetPath
$shortcut.WorkingDirectory = "$scriptDirectory"
$shortcut.Save()
Write-Host "Success: $shortcutFilePath"
$htaPath ="$scriptDirectory"+ "\" + "rest.hta"
# New-Item -Path "$htaPath" -ItemType "File"
$line1 = "<HTA:Application WindowState='maximize' selection='no' Caption='no' scroll='no' SysMenu='no' ShowInTaskBar='no' contextmenu='no'><html><body style='background:black;cursor:url(./black.cur)'></body></html>"
# Set-Content -Path "$htaPath" -Value "$line1"
$line1 | Out-File -FilePath "$htaPath" -Force