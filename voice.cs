using System.Speech.Synthesis;
using System;
using System.Speech.AudioFormat;
class Program {
  static void Main(string[] args) {
SpeechSynthesizer synthesizer = new SpeechSynthesizer();
synthesizer.Rate=-5;
synthesizer.SelectVoice("Microsoft Zira Desktop");
synthesizer.Speak("Force a 10 minute break!"); // 朗读文本
Console.WriteLine(synthesizer.GetInstalledVoices());
foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
        {
          VoiceInfo info = voice.VoiceInfo;
          string AudioFormats = "";
          foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
          {
            AudioFormats += String.Format("{0}\n",
            fmt.EncodingFormat.ToString());
          }

          Console.WriteLine(" Name:          " + info.Name);
          Console.WriteLine(" Culture:       " + info.Culture);
          Console.WriteLine(" Age:           " + info.Age);
          Console.WriteLine(" Gender:        " + info.Gender);
          Console.WriteLine(" Description:   " + info.Description);
          Console.WriteLine(" ID:            " + info.Id);
          Console.WriteLine(" Enabled:       " + voice.Enabled);
          if (info.SupportedAudioFormats.Count != 0)
          {
            Console.WriteLine( " Audio formats: " + AudioFormats);
          }
          else
          {
            Console.WriteLine(" No supported audio formats found");
          }

          string AdditionalInfo = "";
          foreach (string key in info.AdditionalInfo.Keys)
          {
            AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
          }

          Console.WriteLine(" Additional Info - " + AdditionalInfo);
          Console.WriteLine();
        }
// synthesizer.Volume = 100;
// synthesizer.SetOutputToDefaultAudioDevice(); // 设置输出为默认音频设备

  }
}
// using System;
// using System.Threading;
// using System.Windows.Forms;
// using SpeechLib;

// namespace WindowsFormsApplication1 {
//     public partial class Form1 : Form {
//         public Form1() {
//             InitializeComponent();
//         }

//         private void btnSave_Click(object sender, EventArgs e) {
//             SpFileStream stream = new SpFileStream();
//             stream.Open(@"F:\\voice.wav", SpeechStreamFileMode.SSFMCreateForWrite, false);
//             SpVoice voice = new SpVoice();
//             voice.AudioOutputStream = stream;
//             voice.Speak("0,1,2,3,4,5,6,7,8,9");
//             voice.WaitUntilDone(Timeout.Infinite);
//             stream.Close();
//             MessageBox.Show("ok");
//         }
//     }
// }
