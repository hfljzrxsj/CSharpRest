Add-Type -AssemblyName System.Windows.Forms
[System.Windows.Forms.MessageBox]::Show('System.Windows.Forms.MessageBox')
Add-Type -AssemblyName System.Windows.Forms
$dialogResult = [System.Windows.Forms.MessageBox]::Show('System.Windows.Forms.MessageBox', 'confirm', 'YesNo')
if($dialogResult -eq 'Yes'){
    Write-Host 'yes'
} else {
    Write-Host 'no'
}
$notifyIcon = New-Object System.Windows.Forms.NotifyIcon
$notifyIcon.Icon = [System.Drawing.SystemIcons]::Information
$notifyIcon.Text = "notifyIcon.Text"
$notifyIcon.Visible = $true
$notifyIcon_MouseMove = {
    # $time = Get-Date -Format 'HH:mm:ss'
    $time = Get-Date
    $notifyIcon.Text = "time:$time"
}
$notifyIcon.add_MouseMove($notifyIcon_MouseMove)
# 阻止脚本退出
while ($true) {
    Start-Sleep -Milliseconds 1000
}
# 清理资源
$notifyIcon.Dispose()
