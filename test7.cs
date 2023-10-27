using System.Management;
using System.Threading;
using System;
class Program {
  static ManagementScope scope = new ManagementScope("root\\WMI");
  static SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
  static byte currentBrightness = 0;
  static byte GetBrightness() {
    using(ManagementObjectSearcher brightnessSearcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT CurrentBrightness FROM WmiMonitorBrightness"))) {
      foreach(ManagementObject brightnessObj in brightnessSearcher.Get()) {
          return (byte) brightnessObj["CurrentBrightness"];//int// 我们只需要第一个监视器的亮度级别
        } // 获取当前亮度级别
    }
    return 0;
  }
  static void SetBrightnessZero() {
    // 设置亮度为目标亮度级别
    using(ManagementObjectSearcher methodsSearcher = new ManagementObjectSearcher(scope, query)) {
        using(ManagementObjectCollection objectCollection = methodsSearcher.Get()) {
          foreach(ManagementObject methodObj in objectCollection) {
            methodObj.InvokeMethod("WmiSetBrightness", new object[] {
              1, 0
            }); // 第一个参数（1）是超时（以秒为单位），第二个参数是亮度级别
            break; // 我们只需要更改第一个监视器的亮度
          }
        }
      } // 延时一段时间，例如使用 Thread.Sleep 或 Task.Delay 方法等
  }
  static void SetBrightnessOld() { // 恢复亮度为之前的亮度级别
    using(ManagementObjectSearcher methodsSearcher = new ManagementObjectSearcher(scope, query)) {
      using(ManagementObjectCollection objectCollection = methodsSearcher.Get()) {
        foreach(ManagementObject methodObj in objectCollection) {
          methodObj.InvokeMethod("WmiSetBrightness", new object[] {
            1, currentBrightness
          }); // 第一个参数（1）是超时（以秒为单位），第二个参数是亮度级别
          break; // 我们只需要更改第一个监视器的亮度
        }
      }
    }
  }
  static void Main(string[] args) {
    currentBrightness=GetBrightness();
    SetBrightnessZero();
    Thread.Sleep(3000);
    SetBrightnessOld();
  }
}