﻿using Dota_2_Stats.API.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dota_2_Stats.API
{
    public class OpenDotaAPI
    {
        public async Task<PlayerData> GetPlayerData(string playerID)
        {
            RequestHandler handler = new RequestHandler();
            string result =await handler.GETAsync($"https://api.opendota.com/api/players/{playerID}");

            if (result != null)
                return JsonConvert.DeserializeObject<PlayerData>(result);
            else
                return null;
        }

        public async Task<Match[]> GetPlayerRecentMatches(string playerID)
        {
            RequestHandler handler = new RequestHandler();
            string requestString = $@"https://api.opendota.com/api/players/{playerID}/recentMatches";

            string result = await handler.GETAsync(requestString);

            if (result != null)
            {
                return JsonConvert.DeserializeObject<Match[]>(result);
            }
            else
            {
                return null;
            }
                
        }

        public string GetLastLobby(string filePath)
        {
            var ServerLog = new List<string>();

            using (var fs = OpenStream(filePath, FileAccess.Read, FileShare.Read, 3))
            using (StreamReader sr = new StreamReader(fs, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    ServerLog.Add(sr.ReadLine());
                }
            }

            return ServerLog.Last(x => x.Contains("Lobby"));
        }

        private FileStream OpenStream(String fileName, FileAccess fileAccess, FileShare fileShare, int retryCount)
        {
            FileStream fs = null;
            for (int i = 1; i <= 3; ++i)
            {
                try
                {
                    fs = new FileStream(fileName, FileMode.Open, fileAccess, fileShare);
                    break;
                }
                catch (Exception e)
                {
                    if (i == 3)
                        throw e;

                    Thread.Sleep(200);
                }
            }

            if (fs.Length > 0)
            {
                return fs;
            }
            if (retryCount > 0)
            {
                Thread.Sleep(50);
                return OpenStream(fileName, fileAccess, fileShare, retryCount--);
            }
            else
                throw new Exception("File can't be read");
        }

        public List<string> GetPlayerIDs()
        {
            var GameInfo = GetLastLobby(FileManagement.ServerLog);

            var playerStartIndex = GameInfo.IndexOf('(') + 1;
            var playerEndIndex = GameInfo.IndexOf(')');
            var PlayerSection = GameInfo.Substring(playerStartIndex, playerEndIndex - playerStartIndex);

            var Players = PlayerSection.Split(' ').Where(x => x.Contains("[U:")).Take(10).ToList();

            var Results = new List<string>();

            foreach (var item in Players)
            {
                var startIndex = item.LastIndexOf(':') + 1;
                var endIndex = item.IndexOf(']');
                var length = endIndex - startIndex;

                Results.Add(item.Substring(startIndex, length));
            }

            return Results;
        }
    }
}
