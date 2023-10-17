using System.IO;  
  
class Program  
{  
    static void Main()  
    {  
        string path = @"./rest.hta"; // 更改为你想要的路径  
        string content = @"<HTA:Application WindowState='maximize' selection='no' Caption='no' scroll='no' SysMenu='no' ShowInTaskBar='no' contextmenu='no'><html><body style='background:black;cursor:url(./black.cur)'></body></html>"; // 你想要写入的内容  
        // 将内容写入文件，如果文件不存在，会自动创建  
        File.WriteAllText(path, content);  
    }  
}