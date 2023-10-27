using System;
using System.Management;
using System.Threading;

class Program
{
    static void Main()
    {
        // 获取当前的屏幕亮度
        var brightness = GetCurrentBrightness();

        // 将屏幕亮度设置为最低
        SetBrightness(0);

        // 等待5分钟
        Thread.Sleep(TimeSpan.FromMinutes(5));

        // 恢复之前的屏幕亮度
        SetBrightness(brightness);
    }

    static int GetCurrentBrightness()
    {
        using (var mclass = new ManagementClass("WmiMonitorBrightness"))
        {
            using (var instances = mclass.GetInstances())
            {
                foreach (ManagementObject instance in instances)
                {
                    return (byte)instance.GetPropertyValue("CurrentBrightness");
                }
            }
        }

        throw new Exception("无法获取当前的屏幕亮度");
    }

    static void SetBrightness(int brightness)
    {
        using (var mclass = new ManagementClass("WmiMonitorBrightnessMethods"))
        {
            using (var instances = mclass.GetInstances())
            {
                foreach (ManagementObject instance in instances)
                {
                    instance.InvokeMethod("WmiSetBrightness", new object[] { uint.MaxValue, brightness });
                    return;
                }
            }
        }

        throw new Exception("无法设置屏幕亮度");
    }
}
