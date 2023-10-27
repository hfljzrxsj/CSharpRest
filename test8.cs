using System;
class Program
{
    static void Main(string[] args)
    {
        DateTime startTime = DateTime.Now; // 记录开始时间
        Console.WriteLine("请输入任意内容后按回车键：");
        Console.ReadLine(); // 等待用户输入
        TimeSpan elapsedTime = DateTime.Now - startTime; // 计算已经过去的时间
        int elapsedMinutes = (int)elapsedTime.TotalMinutes; // 将时间转换为分钟数（向下取整）
        Console.WriteLine("已经过去了 {0} 分钟。", elapsedMinutes);
        Console.ReadKey();
    }
}