using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Speech.Synthesis;
using System.Management;
class Program {
  [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
  private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  private static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
  private
  const int WM_CLOSE = 0x10;
  [DllImport("user32.dll")]
  private static extern IntPtr GetForegroundWindow();
  [DllImport("user32.dll")]
  private static extern bool SetForegroundWindow(IntPtr hWnd);
  private static NotifyIcon notifyIcon = new NotifyIcon();
  private static int maxTime = 4;
  private static int counter = 40;
  private
  const string htaPath = "rest.hta";
  private
  const int fivemin = 300 * 1000;
  private
  const int onemin = 60 * 1000;
  private
  const string restTitle = "rest now!";
  private static SpeechSynthesizer synthesizer = new SpeechSynthesizer();
  private static ManagementScope scope = new ManagementScope("root\\WMI");
  private static SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
  private static byte currentBrightness = 0;
  private static byte GetBrightness() {
    using(ManagementObjectSearcher brightnessSearcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT CurrentBrightness FROM WmiMonitorBrightness"))) {
      foreach(ManagementObject brightnessObj in brightnessSearcher.Get()) {
          return(byte) brightnessObj["CurrentBrightness"]; // 我们只需要第一个监视器的亮度级别
        } // 获取当前亮度级别
    }
    return 0;
  }
  private static void SetBrightnessZero() {
    using(ManagementObjectSearcher methodsSearcher = new ManagementObjectSearcher(scope, query)) {
        using(ManagementObjectCollection objectCollection = methodsSearcher.Get()) {
          foreach(ManagementObject methodObj in objectCollection) {
            methodObj.InvokeMethod("WmiSetBrightness", new object[] {
              1, 0
            }); // 第一个参数（1）是超时（以秒为单位），第二个参数是亮度级别
            break; // 我们只需要更改第一个监视器的亮度
          }
        }
      } // 延时一段时间，例如使用 Thread.Sleep 或 Task.Delay 方法等
  }
  private static void SetBrightnessOld() { // 恢复亮度为之前的亮度级别
    using(ManagementObjectSearcher methodsSearcher = new ManagementObjectSearcher(scope, query)) {
      using(ManagementObjectCollection objectCollection = methodsSearcher.Get()) {
        foreach(ManagementObject methodObj in objectCollection) {
          methodObj.InvokeMethod("WmiSetBrightness", new object[] {
            1, currentBrightness
          }); // 第一个参数（1）是超时（以秒为单位），第二个参数是亮度级别
          break; // 我们只需要更改第一个监视器的亮度
        }
      }
    }
  }
  private static void randomRest() {
    Random random = new Random();
    double randomNumber = random.NextDouble();
    int result = (int)(randomNumber * (4 - maxTime) * 1.25) + 5;
    // StartProcess("powershell.exe", "-Command \"& {$currentBrightness = (Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightness).CurrentBrightness; $monitor = Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightnessMethods; $monitor.WmiSetBrightness(1, 0); Start-Sleep -Seconds " + (result * 60).ToString() + "; $monitor.WmiSetBrightness(1, $currentBrightness)}\"");
    currentBrightness = GetBrightness();
    RunREST(result);
  }
  private static void init() {
    maxTime = 4;
    counter = 40;
  }
  private static void StartProcess(string fileName, string arguments = null) {
    try {
      Process.Start(fileName, arguments);
    } catch(Exception ex) {
      MessageBox.Show(ex.Message);
    }
  }
  private static void toIcon() {
    int text = counter;
    int maxTime_5 = maxTime * 5;
    if(counter > 0 && maxTime >= 4) text = counter;
    else if(counter >= 0) text = counter + maxTime_5;
    else text = maxTime_5;
    using(Bitmap bitmap = new Bitmap(32, 32)) {
      using(Graphics graphics = Graphics.FromImage(bitmap)) {
        Font font = new Font("Comic Sans MS", 16);
        Brush brush = Brushes.Black;
        graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        graphics.DrawString(text.ToString(), font, brush, new PointF(0, 0));
      }
      using(Icon icon = Icon.FromHandle(bitmap.GetHicon())) {
        notifyIcon.Icon = icon;
      }
    }
  }
  private static string minutes_left(int time) {
      return string.Format("{0} minutes left!", time);
    }
    [STAThread]
  private static async Task run() {
    Thread counterThread = new Thread(() => {
      notifyIcon.Icon = SystemIcons.Information;
      toIcon();
      notifyIcon.BalloonTipTitle = notifyIcon.BalloonTipText = counter.ToString();
      notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
      notifyIcon.BalloonTipClicked += (sender, e) => {
        randomRest();
      };
      // this.Click += new EventHandler(Form1_Click);
      notifyIcon.Text = counter.ToString();
      notifyIcon.Visible = true;
      notifyIcon.MouseMove += NotifyIcon_MouseMove;
      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      contextMenuStrip.Items.Add(restTitle, null, (sender, e) => {
        randomRest();
      });
      notifyIcon.MouseUp += (sender, e) => {
        // if (e.Button == MouseButtons.Left)
        // {
        MethodInfo methodInfo = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
        methodInfo.Invoke(notifyIcon, null);
        // }
      };
      notifyIcon.MouseDoubleClick += (sender, e) => {
        randomRest();
      };
      notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] {
        new MenuItem(restTitle, (sender, e) => {
          randomRest();
        })
      });
      Application.Run();
    });
    counterThread.Start();
    while(true) {
      if(maxTime == 0 && counter <= 0) {
        const string Forced_10_minutes_rest = "Forced 10 minutes rest!";
        notifyIcon.BalloonTipTitle = notifyIcon.BalloonTipText = Forced_10_minutes_rest;
        notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
        notifyIcon.ShowBalloonTip(fivemin);
        synthesizer.Speak(Forced_10_minutes_rest + "Must rest now!");
        currentBrightness = GetBrightness();
        RunREST(10);
        // StartProcess("powershell.exe", "-Command \"& {$currentBrightness = (Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightness).CurrentBrightness; $monitor = Get-WmiObject -Namespace root/WMI -Class WmiMonitorBrightnessMethods; $monitor.WmiSetBrightness(1, 0); Start-Sleep -Seconds 600; $monitor.WmiSetBrightness(1, $currentBrightness)}\"");
      } else if(counter <= 0) {
        string minutes_left_5 = minutes_left(maxTime * 5);
        Task < DialogResult > task = Task.Run(() => {
          return MessageBox.Show(minutes_left_5, restTitle, MessageBoxButtons.OKCancel);
        });
        DateTime startTime = DateTime.Now;
        synthesizer.Speak("It's time to have a rest!" + minutes_left_5);
        if(await Task.WhenAny(task, Task.Delay(fivemin)) == task) {
          DialogResult result = task.Result;
          switch(result) {
            case DialogResult.OK:
              randomRest();
              break;
            case DialogResult.Cancel:
              TimeSpan elapsedTime = DateTime.Now - startTime;
              int elapsedMinutes = (int) elapsedTime.TotalMinutes;
              counter = 5 - elapsedMinutes;
              string watch_for_more_minutes = string.Format("Watch for {0} more minutes!", counter);
              notifyIcon.BalloonTipTitle = notifyIcon.BalloonTipText = watch_for_more_minutes;
              notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
              notifyIcon.ShowBalloonTip(fivemin);
              synthesizer.Speak(watch_for_more_minutes);
              maxTime--;
              break;
          }
        } else {
          IntPtr ptr = FindWindow(null, restTitle);
          if(ptr != IntPtr.Zero) {
            PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
          }
          maxTime--;
          minutes_left_5 = minutes_left(maxTime * 5);
          notifyIcon.BalloonTipTitle = notifyIcon.BalloonTipText = minutes_left_5;
          notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
          notifyIcon.ShowBalloonTip(fivemin);
          synthesizer.Speak(minutes_left_5);
        }
      } else {
        Thread.Sleep(onemin);
        counter--;
      }
      toIcon();
    }
  }
  private static void NotifyIcon_MouseMove(object sender, MouseEventArgs e) {
    NotifyIcon notifyIcon = (NotifyIcon) sender;
    if(counter > 0 && maxTime >= 4) notifyIcon.Text = string.Format("{0} min", counter);
    else if(counter >= 0) notifyIcon.Text = minutes_left(counter + maxTime * 5);
    else notifyIcon.Text = minutes_left(maxTime * 5);
  }
  private static void Main(string[] args) {
    synthesizer.Rate = -5;
    synthesizer.SelectVoice("Microsoft Zira Desktop");
    init();
    Task.Run(async() => await run()).Wait();
  }
  private static void killAll(string name) {
    Process[] processes = Process.GetProcessesByName(name);
    foreach(var process in processes) {
      process.Kill();
    }
  }
  private static void RunREST(int maxCount) {
    init();
    killAll("mshta");
    killAll("mintty");
    killAll("git-bash");
    StartProcess(htaPath);
    // StartProcess("D:/Git/git-bash.exe"); //todo
    if(maxCount > 0) {
      string Rest_for_minutes_left = string.Format("Rest for {0} minutes left!", maxCount);
      notifyIcon.BalloonTipTitle = notifyIcon.BalloonTipText = Rest_for_minutes_left;
      notifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
      notifyIcon.ShowBalloonTip(fivemin);
      synthesizer.Speak(Rest_for_minutes_left);
      // Thread.Sleep(60000);
      int count60 = 60;
      while(true) {
        count60--;
        Thread.Sleep(1000);
        Process[] processes = Process.GetProcessesByName("mshta");
        if(processes.Length > 0) {
          Process notepadProcess = processes[0];
          IntPtr mainWindowHandle = notepadProcess.MainWindowHandle;
          if(GetForegroundWindow() != mainWindowHandle) {
            SetForegroundWindow(mainWindowHandle);
          }
        } else if(processes.Length <= 0) {
          StartProcess(htaPath);
        }
        if(GetBrightness() != 0) {
          SetBrightnessZero();
        }
        if(count60 <= 0) {
          break;
        }
      }
      RunREST(maxCount - 1);
    } else {
      SetBrightnessOld();
      const string Rest_over = "Rest over!";
      notifyIcon.BalloonTipTitle = notifyIcon.BalloonTipText = Rest_over;
      notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
      notifyIcon.ShowBalloonTip(fivemin);
      synthesizer.Speak(Rest_over);
      killAll("mintty");
      killAll("git-bash");
    }
  }
}