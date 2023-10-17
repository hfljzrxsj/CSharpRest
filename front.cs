using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    static void Main()
    {
        // 检测是否存在 notepad.exe 进程
        Process[] processes = Process.GetProcessesByName("mshta");
        if (processes.Length > 0)
        {
            // 获取第一个 notepad.exe 进程
            Process notepadProcess = processes[0];
            IntPtr mainWindowHandle = notepadProcess.MainWindowHandle;
            Console.WriteLine("hhh");
            // 检测 notepad 是否在前台运行
            if (GetForegroundWindow() != mainWindowHandle)
            {
                // 如果 notepad 不在前台，则将其窗口调至前台
                SetForegroundWindow(mainWindowHandle);
            foreach(var process in processes) {
              process.Kill();
            }
            // Process.Start("I:/html/rest.hta");
            }
        }
        else
        {
            // 如果不存在 notepad.exe 进程，则创建一个新的 notepad.exe 进程
            Process.Start("I:/html/rest.hta");
        }
    }
}
