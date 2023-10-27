using System.Diagnostics;

class Program
{
    static void Main()
    {
        ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"HIT-WLAN\" admin=disable")
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            RedirectStandardOutput = true,
        };

        Process proc = new Process() { StartInfo = psi };
        proc.Start();
    }
}
