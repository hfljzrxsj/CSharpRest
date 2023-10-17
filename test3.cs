using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
// using System.Threading;
class Program {
    [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
    private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    public const int WM_CLOSE = 0x10;
  static void StartKiller() {
    Timer timer = new Timer();
    timer.Interval = 3000; //3秒启动 
    timer.Tick += new EventHandler(Timer_Tick);
    timer.Start();
  }
  static void Timer_Tick(object sender, EventArgs e) {
    // KillMessageBox();
    IntPtr ptr = FindWindow(null, "MessageBox");
    if(ptr != IntPtr.Zero) {
      //找到则关闭MessageBox窗口 
      PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
    }
    Console.WriteLine(ptr);
    //停止Timer 
    ((Timer) sender).Stop();
  }
  static void KillMessageBox() {
    //按照MessageBox的标题，找到MessageBox的窗口 
    IntPtr ptr = FindWindow(null, "MessageBox");
    if(ptr != IntPtr.Zero) {
      //找到则关闭MessageBox窗口 
      PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
    }
    
  }
  static void Main(string[] args) {
    StartKiller();
    MessageBox.Show("3秒钟后自动关闭MessageBox窗口", "MessageBox");
    IntPtr ptr = FindWindow(null, "MessageBox");
    if(ptr != IntPtr.Zero) {
      //找到则关闭MessageBox窗口 
      PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
    }
    Console.WriteLine(ptr);
    // Thread.Sleep(3000);
    // KillMessageBox();
  }
}