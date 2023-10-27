using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
class Program {
    static void Main(string[] args) {
      try{
        string currentDirectory = Directory.GetCurrentDirectory();
        string scriptPath = Path.Combine(currentDirectory, "light.ps1");
        Console.WriteLine(scriptPath);
        Process.Start("powershell.exe", "-file \"" + scriptPath + "\"");
      }
      catch(Exception ex) {
      MessageBox.Show(ex.Message);
    }
  }
}