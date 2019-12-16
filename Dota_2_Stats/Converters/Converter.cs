using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Dota_2_Stats.Models;
using System.IO;
using System.Windows.Media;

namespace Dota_2_Stats.Converters
{
    public class Converter
    {
        Dictionary<int, GameMode> gameModeDict = new Dictionary<int, GameMode>();
        Dictionary<int, Hero> heroDict = new Dictionary<int, Hero>();

        public Converter()
        {
            SetGameModeDict();
            SetHeroDict();
        }

        private void SetHeroDict()
        {
            List<Hero> videogames = new List<Hero>();
            using (StreamReader file = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "/Resources/HeroInfo.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                videogames = (List<Hero>)serializer.Deserialize(file, typeof(List<Hero>));
            }

            foreach (Hero mode in videogames)
            {
                heroDict.Add(mode.id, mode);
            }
        }

        private void SetGameModeDict()
        {
            List<GameMode> videogames = new List<GameMode>();
            using (StreamReader file = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "/Resources/GameModeInfo.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                videogames = (List<GameMode>)serializer.Deserialize(file, typeof(List<GameMode>));
            }

            foreach(GameMode mode in videogames)
            {
                gameModeDict.Add(mode.id, mode);
            }
        }

        public string GetHeroName(int HeroID)
        {
            Hero hero = new Hero();
            heroDict.TryGetValue(HeroID, out hero);
            string name = hero.localized_name.Replace("npc_dota_hero_", "");
            name = name.Replace("_", " ");
            return name;
        }

        public BitmapImage GetHeroImg(int HeroID)
        {
            Hero hero = new Hero();
            heroDict.TryGetValue(HeroID, out hero);

            string heroName = hero.localized_name.Replace(" ", "_");
            BitmapImage image = new BitmapImage();
            try
            {
                image.BeginInit();
                image.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + $"/Resources/Icons/{heroName}.png");
                image.EndInit();
                image.Freeze();
            }
            catch
            {
                image.EndInit();
            }
            return image;
        }

        public string GetGameMode(int GameModeID)
        {
            GameMode gameMode = new GameMode();
            gameModeDict.TryGetValue(GameModeID, out gameMode);

            return gameMode.name;
        }

        public SolidColorBrush ToGameBrush(bool win)
        {
            if (win)
            {
                SolidColorBrush brush = new SolidColorBrush(Colors.Green);
                brush.Freeze();
                return brush;
            }
            SolidColorBrush brushLos = new SolidColorBrush(Colors.Red);
            brushLos.Freeze();
            return brushLos;
        }
    }
}
