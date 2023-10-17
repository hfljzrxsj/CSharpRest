# 加载System.IO程序集
Add-Type -TypeDefinition @"
    using System.IO;

    public static class FileHelper
    {
        public static void WriteToFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public static string ReadFromFile(string path)
        {
            return File.ReadAllText(path);
        }
    }
"@ -ReferencedAssemblies System.IO
Add-Type -TypeDefinition @"
using System.Windows.Forms;

public class MessageBoxExample {
    public static void ShowMessageBox(string message, string title) {
        MessageBox.Show(message, title);
    }
}
"@ -ReferencedAssemblies System.Windows.Forms

[MessageBoxExample]::ShowMessageBox("这是一个系统提示框", "提示")
