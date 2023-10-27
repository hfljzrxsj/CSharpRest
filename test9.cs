using System.Management;
using System;
class Program {
  static void SetBrightness(byte targetBrightness) {
    ManagementScope scope = new ManagementScope("root\\WMI");
    SelectQuery query = new SelectQuery("WmiMonitorBrightnessMethods");
    using(ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query)) {
      using(ManagementObjectCollection objectCollection = searcher.Get()) {
        foreach(ManagementObject mObj in objectCollection) {
          mObj.InvokeMethod("WmiSetBrightness", new object[] {
            1, targetBrightness
          }); // 第一个参数（1）是超时（以秒为单位），第二个参数是亮度级别
          break; // 我们只需要更改第一个监视器的亮度
        }
      }
    }
  }
  static void Main(string[] args) {
    SetBrightness(60);
  }
}