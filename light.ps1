param($time)
$currentBrightness = (Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightness).CurrentBrightness
$monitor = Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightnessMethods
$monitor.WmiSetBrightness(1, 0)
Start-Sleep -Seconds "$time"
$monitor.WmiSetBrightness(1, $currentBrightness)