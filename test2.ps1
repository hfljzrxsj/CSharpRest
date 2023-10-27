# 导入必要的WinAPI函数
Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;

public class BrightnessAPI
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    public static extern int GetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

    [DllImport("gdi32.dll")]
    public static extern bool SetDeviceGammaRamp(IntPtr hDC, ref RAMP lpRamp);

    [DllImport("gdi32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct RAMP
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Red;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Green;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] Blue;
    }
}
"@

# 记录当前亮度
$api = New-Object BrightnessAPI
$hWnd = $api::GetForegroundWindow()
$dc = $api::GetDC($hWnd)
$ramp = New-Object BrightnessAPI+RAMP
$result = $api::GetDeviceGammaRamp($dc, [ref]$ramp)
if ($result -eq 0)
{
    throw "获取当前亮度失败"
}
$initialRamp = $ramp
$api::ReleaseDC($hWnd, $dc)

# 设置亮度为最低
$allZeroesRamp = New-Object BrightnessAPI+RAMP
if (-not [BrightnessAPI]::SetDeviceGammaRamp($dc, [ref]$allZeroesRamp))
{
    throw "设置亮度为最低失败"
}

# 等待5分钟
Start-Sleep -Seconds 300

# 恢复之前的亮度
$api::SetDeviceGammaRamp($dc, [ref]$initialRamp)

Write-Host "亮度已恢复"
