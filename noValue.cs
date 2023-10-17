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