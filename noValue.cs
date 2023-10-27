// using System.IO;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Data;
// using System.Text;
    static int ShowPopup(string text, string caption, MessageBoxButtons buttons)
    {
        // Console.WriteLine($"{caption}: {text}");
        Console.WriteLine(string.Format("{0}: {1}", caption, text));
        Console.WriteLine("1. 点击 OK");
        Console.WriteLine("2. 点击 Cancel");
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1)
            {
                return (int)DialogResult.OK;
            }
            else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2)
            {
                return (int)DialogResult.Cancel;
            }
        }
    }
        static DialogResult ShowPopups(string text, string caption, MessageBoxButtons buttons)
    {
        return MessageBox.Show(text, caption, buttons);
    }
        static void ShellExecute(string path)
    {
        // 在此处执行相应的Shell命令或启动外部程序
        // Console.WriteLine($"执行: {path}");
        Console.WriteLine(string.Format("执行: {0}", path));
    }
    // enum DialogResult
// {
//     OK = 1,
//     Cancel = 2
// }
// enum MessageBoxButtons
// {
//     OKCancel = 1
// }

        // string time = DateTime.Now.ToString();
        // count++;
        // if (count <= maxCount)
        // {
                  // }
        // if (count == maxCount + 1)
        // {
        //     count = 1;
        // }
          // static void writeHta()  
  //   {  
  //       string path = @"./rest.hta"; // 更改为你想要的路径  
  //       string content = @"<HTA:Application WindowState='maximize' selection='no' Caption='no' scroll='no' SysMenu='no' ShowInTaskBar='no' contextmenu='no'><html><body style='background:black;cursor:url(./black.cur)'></body></html>"; // 你想要写入的内容  
  //       // 将内容写入文件，如果文件不存在，会自动创建  
  //       File.WriteAllText(path, content);  
  //   }  

        // Process.Start("powershell.exe", "-file \"" + scriptPath + "\" " +((5 + result)*60).ToString());
      // ProcessStartInfo psi = new ProcessStartInfo("powershell.exe");
      // psi.Arguments = string.Format(@"-Command ""& {{$currentBrightness = (Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightness).CurrentBrightness; $monitor = Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightnessMethods; $monitor.WmiSetBrightness(1, 0); Start-Sleep -Seconds {0}; $monitor.WmiSetBrightness(1, $currentBrightness)}}""",result);
      // Process.Start(psi);
          // StartProcess("powershell.exe -file light.ps1 "+((5 + result)*60).ToString());
              // string currentDirectory = Directory.GetCurrentDirectory();
    // string scriptPath = Path.Combine(currentDirectory, "light.ps1");

      // static void StartKiller(int time) {
  //   System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
  //   timer.Interval = time; //3秒启动 
  //   timer.Tick += new EventHandler(Timer_Tick);
  //   timer.Start();
  // }
  // static void Timer_Tick(object sender, EventArgs e) {
  //   IntPtr ptr = FindWindow(null, restTitle);
  //   if(ptr != IntPtr.Zero) {//找到则关闭MessageBox窗口 
  //     PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
  //   }
  //   ((System.Windows.Forms.Timer) sender).Stop();//停止Timer 
  // }
// StartKiller(fivemin);
// StartKiller(1);
    // writeHta();