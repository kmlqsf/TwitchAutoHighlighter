using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TwitchAutoHighlighter
{
    public class Downloader
    {
        private static int LastHighlightTime = 0;
        public static void DownloadVideo(string id, int count, int startTime, int endTime)
        {
            if (endTime - LastHighlightTime < (endTime - startTime) * 2 && LastHighlightTime != 0)
            {
                startTime = LastHighlightTime;
            }

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "TwitchDownloaderCLI.exe";
            startInfo.Arguments = $"-m VideoDownload --id {id} -o video{count}.mp4 -b {startTime} -e {endTime}";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            LastHighlightTime = endTime;

        }
        public static void DownloadChat(string id)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "TwitchDownloaderCLI.exe";
            startInfo.Arguments = $"-m ChatDownload --id {id} -o chat.json";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}