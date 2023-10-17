using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Data;
// using System.Text;
using System.Reflection;
class Program {
  [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
  private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
  public
  const int WM_CLOSE = 0x10;
  [DllImport("user32.dll")]
  private static extern IntPtr GetForegroundWindow();
  [DllImport("user32.dll")]
  private static extern bool SetForegroundWindow(IntPtr hWnd);
  static NotifyIcon notifyIcon = new NotifyIcon();
  static int maxTime = 4;
  static int counter = 40;
  const string htaPath = "I:/html/rest.hta";
  const int fivemin = 300 * 1000;
  const int onemin = 60;
  const string restTitle = "rest now!";
  static void init() {
    maxTime = 4;
    counter = 40;
  }
  static void StartProcess(string fileName) {
    try {
      Process.Start(fileName);
    } catch(Exception ex) {
      MessageBox.Show(string.Format("无法启动进程：{0}", ex.Message));
    }
  }
  private static void NotifyIcon_BalloonTipClicked(object sender, EventArgs e) {
    // 在气球提示被点击时显示消息框。
    RunREST(5);
  }
  static void StartKiller(int time) {
    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
    timer.Interval = time; //3秒启动 
    timer.Tick += new EventHandler(Timer_Tick);
    timer.Start();
  }
  static void Timer_Tick(object sender, EventArgs e) {
      IntPtr ptr = FindWindow(null, restTitle);
      if(ptr != IntPtr.Zero) {
        //找到则关闭MessageBox窗口 
        PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
      }
      //停止Timer 
      ((System.Windows.Forms.Timer) sender).Stop();
    }
    [STAThread]
  public static async Task run() {
    Thread counterThread = new Thread(() => {
      notifyIcon.Icon = SystemIcons.Information;
      notifyIcon.BalloonTipTitle = counter.ToString();
      notifyIcon.BalloonTipText = counter.ToString();
      notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
      notifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClicked;
      // this.Click += new EventHandler(Form1_Click);
      notifyIcon.Text = counter.ToString();
      notifyIcon.Visible = true;
      // 注册 MouseMove 和 MouseDoubleClick 事件处理程序。
      notifyIcon.MouseMove += NotifyIcon_MouseMove;
      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      contextMenuStrip.Items.Add("现在就休息", null, (sender, e) => {
        RunREST(5);
      });
      notifyIcon.MouseUp += (sender, e) => {
        // if (e.Button == MouseButtons.Left)
        // {
        MethodInfo methodInfo = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
        methodInfo.Invoke(notifyIcon, null);
        // }
      };
      // notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
      // 创建右键菜单。
      ContextMenu contextMenu = new ContextMenu(new MenuItem[] {
        new MenuItem("现在就休息", ContextMenu_OpenNotepad)
      });
      // 将右键菜单分配给 NotifyIcon 对象。
      notifyIcon.ContextMenu = contextMenu;
      Application.Run();
    });
    counterThread.Start();
    while(true) {
      Thread.Sleep(onemin); // 等待1分钟
      counter--;
      if(maxTime == 0) {
        RunREST(10);
      } else if(counter <= 0) {
        // DialogResult result = MessageBox.Show("该休息一下！", "休息提醒", MessageBoxButtons.OKCancel);
        StartKiller(fivemin);
        Task < DialogResult > task = Task.Run(() => {
          return MessageBox.Show(string.Format("还剩{0}分钟", maxTime * 5), restTitle, MessageBoxButtons.OKCancel);
        });
        if(await Task.WhenAny(task, Task.Delay(fivemin)) == task) {
          DialogResult result = task.Result;
          switch(result) {
            case DialogResult.OK: // 点击“马上休息”
              RunREST(5);
              break;
            case DialogResult.Cancel: // 点击“再看5分钟”
              Console.WriteLine("再看5分钟");
              counter = 5;
              maxTime--;
              break;
          }
        } else {
          maxTime--;
          notifyIcon.BalloonTipTitle = string.Format("还剩{0}分钟", maxTime * 5);
          notifyIcon.BalloonTipText = string.Format("还剩{0}分钟", maxTime * 5);
          notifyIcon.ShowBalloonTip(fivemin);
          StartKiller(1);
          IntPtr ptr = FindWindow(null, restTitle);
          if(ptr != IntPtr.Zero) {
            //找到则关闭MessageBox窗口 
            PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
          }
          // Console.WriteLine("再看5分钟");
        }
      }
    }
  }
  private static void NotifyIcon_MouseMove(object sender, MouseEventArgs e) {
    NotifyIcon notifyIcon = (NotifyIcon) sender;
    if(counter > 0 && maxTime >= 4) notifyIcon.Text = string.Format("{0}分钟", counter);
    else if(counter > 0) notifyIcon.Text = string.Format("还剩{0}分钟", counter + maxTime * 5);
    else notifyIcon.Text = string.Format("还剩{0}分钟", maxTime * 5);
  }
  private static void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
    // 双击左键时执行的代码。
  }
  private static void ContextMenu_OpenNotepad(object sender, EventArgs e) {
    // 打开记事本。
    RunREST(5);
  }
  static void Main(string[] args) {
    Task.Run(async() => await run()).Wait();
    //Console.WriteLine("按任意键退出...");
  }
  static void RunREST(int maxCount) {
    init();
    //Console.WriteLine("休息一下");
    StartProcess(htaPath);
    StartProcess("D:/Git/git-bash.exe"); //todo
    if(maxCount > 0) {
      // Thread.Sleep(60000); // 延迟1分钟后再次执行RunREST
      int count60 = 60;
      while(true) {
        count60--;
        Thread.Sleep(1000); // 延迟1秒
        Process[] processes = Process.GetProcessesByName("mshta");
        if(processes.Length > 0) {
          // 获取第一个 notepad.exe 进程
          Process notepadProcess = processes[0];
          IntPtr mainWindowHandle = notepadProcess.MainWindowHandle;
          // 检测 notepad 是否在前台运行
          if(GetForegroundWindow() != mainWindowHandle) {
            // 如果 notepad 不在前台，则将其窗口调至前台
            SetForegroundWindow(mainWindowHandle);
            foreach(var process in processes) {
              process.Kill();
            }
            StartProcess(htaPath);
          }
        } else if(processes.Length <= 0) {
          StartProcess(htaPath);
        }
        if(count60 <= 0) {
          break;
        }
      }
      RunREST(maxCount - 1);
    }
  }
}