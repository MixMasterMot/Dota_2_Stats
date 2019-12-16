using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Dota_2_Stats.API;
using Dota_2_Stats.API.Output;
using Dota_2_Stats.Models;
using Dota_2_Stats.Converters;
using System.Threading;

namespace Dota_2_Stats.API
{
    public class CurrentMatch
    {
        Converter Converter = new Converter();
        public async Task<PlayerModel[]> UpdateCurrentMatch()
        {
            // get player IDs
            OpenDotaAPI dotaAPI = new OpenDotaAPI();
            List<string> playerIDs = dotaAPI.GetPlayerIDs();
            List<Task<PlayerModel>> playerModels = new List<Task<PlayerModel>>();
            for(int i = 0; i < 10; i++)
            {
                playerModels.Add(GetPlayerModel(playerIDs[i]));
            }
            var s = await Task.WhenAll(playerModels);
            return s;
        }

        public async Task<PlayerModel> GetPlayerModel(string playerID)
        {
            // get player data
            OpenDotaAPI dotaAPI = new OpenDotaAPI();
            PlayerData playerData = await dotaAPI.GetPlayerData(playerID);

            // if player not valid return anon player
            var info = ConvertToPlayerModel(playerData);
            if (!info.Item1)
            {
                return info.Item2;
            }

            PlayerModel playerModel = info.Item2;
            // get recent matches
            Match[] matches = await dotaAPI.GetPlayerRecentMatches(playerID);
            playerModel.RecentMatchesObservable = ConvertToRecentMatches(matches);

            return playerModel;
        }

        public (bool,PlayerModel) ConvertToPlayerModel(PlayerData playerData)
        {
            if(playerData==null || playerData.profile == null)
            {
                PlayerModel playerFail = new PlayerModel()
                {
                    Name = "Anonymous"
                };
                return (false, playerFail);
            }

            PlayerModel player = new PlayerModel()
            {
                Name = playerData.profile.personaname,
                Matches = "-1",
                WinRate = "-1",
                MMR = playerData.solo_competitive_rank,
                EstMMR = Convert.ToString(playerData.mmr_estimate.estimate),
                //Avatar = GetAvatar(playerData.profile.avatar, Convert.ToString(playerData.profile.account_id)),
                AvatarPath = GetAvatarPath(playerData.profile.avatar, Convert.ToString(playerData.profile.account_id))
            };
            return (true, player);
        }

        public ObservableCollection<RecentMatch> ConvertToRecentMatches(Match[] matches)
        {
            ObservableCollection<RecentMatch> recentMatchesObservable = new ObservableCollection<RecentMatch>();

            foreach (Match match in matches)
            {
                RecentMatch recentMatch = new RecentMatch()
                {
                    match_id = Convert.ToString(match.match_id),
                    Win = match.Won, 
                    //WinBrush = new Converter().ToGameBrush(match.Won),// requires Converter
                    Duration = Convert.ToString(match.duration),
                    GameMode = new Converter().GetGameMode(match.game_mode), // requires converter
                    HeroName = new Converter().GetHeroName(match.hero_id), // requires converter
                    hero = new Converter().GetHeroImg(match.hero_id), // requires converter
                    Kills = Convert.ToString(match.kills),
                    Deaths = Convert.ToString(match.deaths),
                    Assists = Convert.ToString(match.assists),
                    XPpm = Convert.ToString(match.xp_per_min),
                    Gpm = Convert.ToString(match.gold_per_min),
                    HeroDmg = Convert.ToString(match.hero_damage),
                    TowerDmg = Convert.ToString(match.tower_damage),
                    HeroHeal = Convert.ToString(match.hero_healing),
                    LastHit = Convert.ToString(match.last_hits)
                };
                recentMatchesObservable.Add(recentMatch);
            }
            
            return recentMatchesObservable;
        }

        public BitmapImage GetAvatar(string url, string accID)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            Bitmap bitmap = new Bitmap(stream);

            string filename = AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Temp/{accID}.png";
            if (bitmap != null)
            {
                bitmap.Save(filename, ImageFormat.Png);
            }
            stream.Flush();
            stream.Close();
            client.Dispose();

            BitmapImage bitmapImage = GetAvatarImg(accID);
            
            return bitmapImage;
        }

        public string GetAvatarPath(string url, string accID)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            Bitmap bitmap = new Bitmap(stream);

            string filename = AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Temp/{accID}.png";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            if (bitmap != null)
            {
                bitmap.Save(filename, ImageFormat.Png);
            }
            stream.Flush();
            stream.Close();
            client.Dispose();

            return filename;
        }

        private BitmapImage GetAvatarImg(string accID)
        {
            BitmapImage image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Temp/{accID}.png");
                image.EndInit();
                image.Freeze();
            }
            catch
            {
                image.EndInit();
            }
            return image;
        }

        private BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                //bitmapImage.Freeze();

                return bitmapImage;
            }
        }

    }
}
