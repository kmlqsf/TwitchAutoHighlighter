using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchAutoHighlighter
{
    public class VideoCutter
    {
        public void Cut(int count)
        {
            var path = Directory.GetCurrentDirectory() + "\\text.txt";


            using (StreamWriter sw = File.CreateText(path))
            {
                for (int i = 1; i <= count; i++)
                {
                    sw.WriteLine($"file 'video{i}.mp4'");
                }
                sw.Close();
            }



            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "ffmpeg.exe";
            startInfo.Arguments = $"-f concat -safe 0 -i text.txt -c copy 1.mp4";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        public void DeleteHighlights()
        {
            string pattern = "video*.mp4";
            var dir = Directory.GetCurrentDirectory();
            var matches = Directory.GetFiles(dir, pattern);
            foreach(string file in matches)
                File.Delete(file);
        }
    }
}