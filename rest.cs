using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
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
  const string htaPath = "rest.hta";
  const int fivemin = 300 * 1000;
  const int onemin = 60 * 1000;
  const string restTitle = "rest now!";
  static void randomRest() {
    Random random = new Random();
    double randomNumber = random.NextDouble(); // ����һ��0��1֮��������  
    int result = (int)(randomNumber * (4 - maxTime) * 1.25); // ����10������ȡ��
    RunREST(5 + result);
  }
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
    randomRest();
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
  static void toIcon() {
      int text = counter; // ��������滻Ϊ����Ҫ��ʾ���ı���������Ե���
      if(counter > 0 && maxTime >= 4) text = counter;
      else if(counter >= 0) text = counter + maxTime * 5;
      else text = maxTime * 5;
      // ����һ�� Bitmap ���󣬲������ı�����
      using(Bitmap bitmap = new Bitmap(32, 32)) {
        using(Graphics graphics = Graphics.FromImage(bitmap)) {
            Font font = new Font("Comic Sans MS", 16);
            Brush brush = Brushes.Black;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graphics.DrawString(text.ToString(), font, brush, new PointF(0, 0));
          }
          // �� Bitmap ת��Ϊ Icon
        using(Icon icon = Icon.FromHandle(bitmap.GetHicon())) {
          // �� Icon ��ֵ�� notifyIcon1.Icon
          notifyIcon.Icon = icon;
        }
      }
    }
    [STAThread]
  public static async Task run() {
    Thread counterThread = new Thread(() => {
      notifyIcon.Icon = SystemIcons.Information;
      toIcon();
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
        randomRest();
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
      toIcon();
      if(maxTime == 0) {
        notifyIcon.BalloonTipTitle = "ǿ����Ϣ10����";
        notifyIcon.BalloonTipText = "ǿ����Ϣ10����";
        notifyIcon.ShowBalloonTip(fivemin);
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
              randomRest();
              break;
            case DialogResult.Cancel: // ������ٿ�5���ӡ�
              //Console.WriteLine("�ٿ�5����");
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
    else if(counter >= 0) notifyIcon.Text = string.Format("��ʣ{0}����", counter + maxTime * 5);
    else notifyIcon.Text = string.Format("��ʣ{0}����", maxTime * 5);
  }
  private static void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {
    // ˫�����ʱִ�еĴ��롣
  }
  private static void ContextMenu_OpenNotepad(object sender, EventArgs e) {
    // �򿪼��±���
    randomRest();
  }
  static void Main(string[] args) {
    init();
    // writeHta();
    Task.Run(async() => await run()).Wait();
    //Console.WriteLine("��������˳�...");
  }
  static void killAll(string name) {
    Process[] processes = Process.GetProcessesByName(name);
    foreach(var process in processes) {
      process.Kill();
    }
  }
  static void RunREST(int maxCount) {
    init();
    killAll("mshta");
    killAll("mintty");
    killAll("git-bash");
    //Console.WriteLine("��Ϣһ��");
    StartProcess(htaPath);
    // StartProcess("D:/Git/git-bash.exe"); //todo
    if(maxCount > 0) {
      notifyIcon.BalloonTipTitle = string.Format("��ʣ��Ϣ{0}����", maxCount);
      notifyIcon.BalloonTipText = string.Format("��ʣ��Ϣ{0}����", maxCount);;
      notifyIcon.ShowBalloonTip(fivemin);
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
            // foreach(var process in processes) {
            //   process.Kill();
            // }
            // StartProcess(htaPath);
          }
        } else if(processes.Length <= 0) {
          StartProcess(htaPath);
        }
        if(count60 <= 0) {
          break;
        }
      }
      RunREST(maxCount - 1);
    } else {
      notifyIcon.BalloonTipTitle = "��Ϣ����";
      notifyIcon.BalloonTipText = "��Ϣ����";
      notifyIcon.ShowBalloonTip(fivemin);
      killAll("mintty");
      killAll("git-bash");
    }
  }
}