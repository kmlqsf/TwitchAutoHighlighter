using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwitchAutoHighlighter
{
    public class ChatAnalyzer
    {
        public static string StreamerName;
        public static string StreamStartDate;
        private static int _messagesCount;
        private static JObject _chatJson;

        public ChatAnalyzer()
        {
            _chatJson = ConvertChatToJObject();
            StreamerName = _chatJson["streamer"]?["name"]?.ToString();
            StreamStartDate = Convert.ToDateTime(GetFromComments("created_at").First()).ToString("dd-MM-yyyy");
            _messagesCount = GetFromComments("message").Count();
        }

        public static List<Clip> Analyze(int count, int hSeconds)
        {
            var dates = GetFromComments("created_at").Select(Convert.ToDateTime).ToList(); 
            var seconds = GetFromComments("content_offset_seconds").Select(Convert.ToDecimal).ToList();

            var collectionOfAverageSpeed = new List<Clip>();
            var previousClipLastMessageNumber = 0;
            var messageCount = 0;

            for (var currentClipLastMessageNumber = 0; currentClipLastMessageNumber < _messagesCount; currentClipLastMessageNumber++)
            {
                messageCount += 1;
                if (dates.ElementAt(currentClipLastMessageNumber) > dates.ElementAt(previousClipLastMessageNumber).AddSeconds(hSeconds))
                {
                    previousClipLastMessageNumber = currentClipLastMessageNumber;
                    collectionOfAverageSpeed.Add(new Clip{StartTime = seconds[currentClipLastMessageNumber], MessageCount = messageCount});
                    messageCount = 0;
                }
            }

            return collectionOfAverageSpeed
                .OrderByDescending(clip => clip.MessageCount)
                .Take(count)
                .Select(clip => new Clip {StartTime = clip.StartTime, MessageCount = clip.MessageCount})
                .OrderBy(clip => clip.StartTime)
                .ToList();
        }

        private static IEnumerable<JToken> GetFromComments(string childName)
        {
            return _chatJson["comments"]?.Children()[childName];
        }

        private static JObject ConvertChatToJObject()
        {
            const string chatJsonFileName = "chat.json";
            using var file = File.OpenText(chatJsonFileName);
            using var reader = new JsonTextReader(file);
            return (JObject)JToken.ReadFrom(reader);
        }
    }
}