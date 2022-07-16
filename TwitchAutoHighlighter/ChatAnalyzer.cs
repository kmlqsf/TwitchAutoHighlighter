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
        public List<Tuple<int, int>> Analyze(int count, int hSeconds)
        {
            using var file = File.OpenText(@"chat.json");
            using var reader = new JsonTextReader(file);
            var jToken = (JObject)JToken.ReadFrom(reader);
            var createdAt = jToken["comments"].Children()["created_at"];
            var second = jToken["comments"].Children()["content_offset_seconds"];

            var dates = createdAt.Select(Convert.ToDateTime).ToList();

            var seconds = second.Select(Convert.ToInt32).ToList();


            var collectionOfAverageSpeed = new List<Tuple<int, int>>();
            var x = 0;
            var messageCount = 0;
            var time = 0;
            
            
            
            for (var z = 0; z < dates.Count(); z++)
            {
                messageCount += 1;
                if (dates.ElementAt(z) > dates.ElementAt(x).AddSeconds(hSeconds))
                {
                    x = z;
                    time += hSeconds;
                    collectionOfAverageSpeed.Add(new Tuple<int, int>(seconds[z], messageCount));
                    messageCount = 0;

                }


            }

            collectionOfAverageSpeed = collectionOfAverageSpeed.OrderByDescending(i => i.Item2).ToList();
            collectionOfAverageSpeed = collectionOfAverageSpeed.Take(count).ToList();
            var topHighlights = new List<Tuple<int, int>>();

            for (int i = 0; i < collectionOfAverageSpeed.Count; i++)
            {
                
                
                    topHighlights.Add(new Tuple<int, int>(collectionOfAverageSpeed[i].Item1, collectionOfAverageSpeed[i].Item2));
                


            }

            return topHighlights.OrderBy(i => i.Item1).ToList();

        }
    }
}