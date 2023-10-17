using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
    static void Main()
    {
        string text = "24"; // 这里可以替换为你想要显示的文本，例如电脑电量

        // 创建一个 Bitmap 对象，并绘制文本内容
        using (Bitmap bitmap = new Bitmap(32, 32))
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Font font = new Font("Arial", 16);
                Brush brush = Brushes.Black;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                graphics.DrawString(text, font, brush, new PointF(0, 0));
            }

            // 将 Bitmap 转换为 Icon
            using (Icon icon = Icon.FromHandle(bitmap.GetHicon()))
            {
                // 将 Icon 赋值给 notifyIcon1.Icon
                notifyIcon1.Icon = icon;
            }
        }
    }
}
