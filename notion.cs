using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
class Program
{
//     void Form1_Click(object sender, EventArgs e) 
// {
//     notifyIcon1.Visible = true;
//     notifyIcon1.ShowBalloonTip(30000);
// }
    static NotifyIcon notifyIcon = new NotifyIcon();
    static void Main()
    {
        // 创建 NotifyIcon 对象并设置图标、文本和可见性。
        notifyIcon.Icon = SystemIcons.Information;
        notifyIcon.BalloonTipTitle = "notifyIcon.BalloonTipTitle";
        notifyIcon.BalloonTipText = "notifyIcon.BalloonTipText";
        notifyIcon.BalloonTipIcon=ToolTipIcon.Error;
        // this.Click += new EventHandler(Form1_Click);
        notifyIcon.Text = "notifyIcon.Text";
        notifyIcon.Visible = true;
        // 注册 MouseMove 和 MouseDoubleClick 事件处理程序。
        notifyIcon.MouseMove += NotifyIcon_MouseMove;
        notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
        // 创建右键菜单。
        ContextMenu contextMenu = new ContextMenu(new MenuItem[] {
            new MenuItem("打开记事本", ContextMenu_OpenNotepad)
        });
        // 将右键菜单分配给 NotifyIcon 对象。
        notifyIcon.ContextMenu = contextMenu;
        // 显示计数器的线程。
        Thread counterThread = new Thread(() =>
        {
            int count = 0;
            while (true)
            {
                Console.WriteLine(string.Format("Count: {0}", count));
                Thread.Sleep(5000); // 等待5秒
                count++;
            }
        });
        counterThread.Start();
        // 阻止主线程退出。
        Application.Run();
        Console.WriteLine("Application.Run() returned");
        // 清理资源。
        notifyIcon.Dispose();
    }
    private static void NotifyIcon_MouseMove(object sender, MouseEventArgs e)
    {
        NotifyIcon notifyIcon = (NotifyIcon)sender;
        string time = DateTime.Now.ToString();
        notifyIcon.Text = string.Format("time: {0}", time);
    }
    private static void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        // 双击左键时执行的代码。
    }
    private static void ContextMenu_OpenNotepad(object sender, EventArgs e)
    {
        // 打开记事本。
        Process.Start("notepad.exe");
        notifyIcon.ShowBalloonTip(30000);
    }
}
