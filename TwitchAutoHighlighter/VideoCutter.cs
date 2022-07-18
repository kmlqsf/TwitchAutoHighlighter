using System.IO;

namespace TwitchAutoHighlighter
{
    public class VideoCutter
    {
        public void Cut(int count)
        {
            var path = Directory.GetCurrentDirectory() + "\\text.txt";


            using (var sw = File.CreateText(path))
            {
                for (var i = 0; i < count; i++)
                {
                    sw.WriteLine($"file 'video{i}.mp4'");
                }
                sw.Close();
            }



            var process = new System.Diagnostics.Process();
            var fileName = $"{ChatAnalyzer.StreamerName}-{ChatAnalyzer.StreamStartDate}";
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-f concat -safe 0 -i text.txt -c copy {fileName}.mp4"
            };
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