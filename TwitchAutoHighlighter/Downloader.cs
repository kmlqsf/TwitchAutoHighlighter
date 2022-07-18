using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace TwitchAutoHighlighter
{
    public static class Downloader
    {
        private static decimal _lastHighlightTime = 0;
        public static void DownloadVideo(string id, decimal count, decimal startTime, decimal endTime)
        {
            if (endTime - _lastHighlightTime < (endTime - startTime) * 2 && _lastHighlightTime != 0)
            {
                startTime = _lastHighlightTime;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                FileName = "TwitchDownloaderCLI.exe",
                Arguments = $"-m VideoDownload --id {id} -o video{count}.mp4 -b {Convert.ToInt32(startTime)} -e {Convert.ToInt32(endTime)}"
            }};
            process.Start();
            process.WaitForExit();
            _lastHighlightTime = endTime;

        }
        public static void DownloadChat(string id)
        {            var startInfo = new ProcessStartInfo
            {
                FileName = "TwitchDownloaderCLI.exe",
                Arguments = $"-m ChatDownload --id {id} -o chat.json"
            };

            var process = new Process(){StartInfo = startInfo};
            process.Start();
            process.WaitForExit();
        }
    }
}