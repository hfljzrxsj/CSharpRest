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
      MessageBox.Show(string.Format("�޷��������̣�{0}", ex.Message));
    }
  }
  private static void NotifyIcon_BalloonTipClicked(object sender, EventArgs e) {
    // ��������ʾ�����ʱ��ʾ��Ϣ��
    RunREST(5);
  }
  static void StartKiller(int time) {
    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
    timer.Interval = time; //3������ 
    timer.Tick += new EventHandler(Timer_Tick);
    timer.Start();
  }
  static void Timer_Tick(object sender, EventArgs e) {
      IntPtr ptr = FindWindow(null, restTitle);
      if(ptr != IntPtr.Zero) {
        //�ҵ���ر�MessageBox���� 
        PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
      }
      //ֹͣTimer 
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
      // ע�� MouseMove �� MouseDoubleClick �¼��������
      notifyIcon.MouseMove += NotifyIcon_MouseMove;
      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      contextMenuStrip.Items.Add("���ھ���Ϣ", null, (sender, e) => {
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
      // �����Ҽ��˵���
      ContextMenu contextMenu = new ContextMenu(new MenuItem[] {
        new MenuItem("���ھ���Ϣ", ContextMenu_OpenNotepad)
      });
      // ���Ҽ��˵������ NotifyIcon ����
      notifyIcon.ContextMenu = contextMenu;
      Application.Run();
    });
    counterThread.Start();
    while(true) {
      Thread.Sleep(onemin); // �ȴ�1����
      counter--;
      if(maxTime == 0) {
        RunREST(10);
      } else if(counter <= 0) {
        // DialogResult result = MessageBox.Show("����Ϣһ�£�", "��Ϣ����", MessageBoxButtons.OKCancel);
        StartKiller(fivemin);
        Task < DialogResult > task = Task.Run(() => {
          return MessageBox.Show(string.Format("��ʣ{0}����", maxTime * 5), restTitle, MessageBoxButtons.OKCancel);
        });
        if(await Task.WhenAny(task, Task.Delay(fivemin)) == task) {
          DialogResult result = task.Result;
          switch(result) {
            case DialogResult.OK: // �����������Ϣ��
              RunREST(5);
              break;
            case DialogResult.Cancel: // ������ٿ�5���ӡ�
              Console.WriteLine("�ٿ�5����");
              counter = 5;
              maxTime--;
              break;
          }
        } else {
          maxTime--;
          notifyIcon.BalloonTipTitle = string.Format("��ʣ{0}����", maxTime * 5);
          notifyIcon.BalloonTipText = string.Format("��ʣ{0}����", maxTime * 5);
          notifyIcon.ShowBalloonTip(fivemin);
          StartKiller(1);
          IntPtr ptr = FindWindow(null, restTitle);
          if(ptr != IntPtr.Zero) {
            //�ҵ���ر�MessageBox���� 
            PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
          }
          // Console.WriteLine("�ٿ�5����");
        }
      }
    }
  }
  private static void NotifyIcon_MouseMove(object sender, MouseEventArgs e) {
    NotifyIcon notifyIcon = (NotifyIcon) sender;
    if(counter > 0 && maxTime >= 4) notifyIcon.Text = string.Format("{0}����", counter);
    else if(counter > 0) notifyIcon.Text = string.Format("��ʣ{0}����", counter + maxTime * 5);
    else notifyIcon.Text = string.Format("��ʣ{0}����", maxTime * 5);
  }
  private static void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
    // ˫�����ʱִ�еĴ��롣
  }
  private static void ContextMenu_OpenNotepad(object sender, EventArgs e) {
    // �򿪼��±���
    RunREST(5);
  }
  static void Main(string[] args) {
    Task.Run(async() => await run()).Wait();
    //Console.WriteLine("��������˳�...");
  }
  static void RunREST(int maxCount) {
    init();
    //Console.WriteLine("��Ϣһ��");
    StartProcess(htaPath);
    StartProcess("D:/Git/git-bash.exe"); //todo
    if(maxCount > 0) {
      // Thread.Sleep(60000); // �ӳ�1���Ӻ��ٴ�ִ��RunREST
      int count60 = 60;
      while(true) {
        count60--;
        Thread.Sleep(1000); // �ӳ�1��
        Process[] processes = Process.GetProcessesByName("mshta");
        if(processes.Length > 0) {
          // ��ȡ��һ�� notepad.exe ����
          Process notepadProcess = processes[0];
          IntPtr mainWindowHandle = notepadProcess.MainWindowHandle;
          // ��� notepad �Ƿ���ǰ̨����
          if(GetForegroundWindow() != mainWindowHandle) {
            // ��� notepad ����ǰ̨�����䴰�ڵ���ǰ̨
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