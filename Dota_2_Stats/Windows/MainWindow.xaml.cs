using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

using Dota_2_Stats.ViewModels;
using Dota_2_Stats.Models;
using Dota_2_Stats.PopUps;
using Dota_2_Stats.API;
using Dota_2_Stats.Converters;

using System.IO;
using System.ComponentModel;
using Microsoft.Win32;

namespace Dota_2_Stats.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        PlayersViewmodel PlayersViewmodel = new PlayersViewmodel();
        RecentGamesPopUp RecentMatch_Popup = new RecentGamesPopUp();
        PlayerPopUp PlayerPopUp = new PlayerPopUp();

        public MainWindow()
        {
            InitializeComponent();
            tst();
            GenerateGrid(RecentGamesGrid);
            SetupFileWatcher();
            MasterGrid.DataContext = PlayersViewmodel.PlayerModelsObservable;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            RecentMatch_Popup._popUp.HorizontalOffset += 1;
            RecentMatch_Popup._popUp.HorizontalOffset -= 1;

            PlayerPopUp._popUp.HorizontalOffset += 1;
            PlayerPopUp._popUp.HorizontalOffset -= 1;

            base.OnLocationChanged(e);
        }

        private string PreviousLobby = null;
        private void SetupFileWatcher()
        {
            OpenDotaAPI dotaAPI = new OpenDotaAPI();

            PreviousLobby = dotaAPI.GetLastLobby(FileManagement.ServerLog);

            FileSystemWatcher watcher = new FileSystemWatcher(new FileInfo(FileManagement.ServerLog).Directory.FullName)
            {
                EnableRaisingEvents = true
            };

            watcher.Changed += (newobject, newargs) =>
            {
                try
                {
                    string tempLobby = dotaAPI.GetLastLobby(FileManagement.ServerLog);
                    if (PreviousLobby != tempLobby)
                    {
                        watcher.EnableRaisingEvents = false;

                        UpdatePlayers();

                        PreviousLobby = tempLobby;
                    }
                }
                finally
                {
                    watcher.EnableRaisingEvents = true;
                }
            };
        }

        private void GenerateGrid(Grid grid)
        {
            int playerPos = 0;
            for(int i = 1; i < 12; i++)
            {
                if (i == 6) continue;
                for (int x = 0; x < 20; x++)
                {
                    Binding borderBind = new Binding()
                    {
                        Path = new PropertyPath($"[{playerPos}].RecentMatchesObservable[{x}].Win"),
                        Converter = new ToWinBrushConverter(),
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };

                    Border border = new Border()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Pink),
                        BorderThickness = new Thickness(2),
                    };

                    BindingOperations.SetBinding(border, Border.BackgroundProperty, borderBind);

                    Image image = new Image();
                    image.MouseLeftButtonUp += new MouseButtonEventHandler(RecentMatch_MouseLeftButtonUp);
                    
                    Binding bind = new Binding()
                    {
                        Path = new PropertyPath($"[{playerPos}].RecentMatchesObservable[{x}].hero"),
                        IsAsync=true,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    BindingOperations.SetBinding(image, Image.SourceProperty, bind);
                    border.Child = image;

                    Grid.SetColumn(border, x);
                    Grid.SetRow(border, i);
                    grid.Children.Add(border);
                }
                playerPos++;
            }
        }

        private void RecentMatch_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            
            Binding binding = BindingOperations.GetBinding(img, Image.SourceProperty);
            string path = Convert.ToString(binding.Path.Path);

            //"[x].RecentMatchesObservable[i].hero"
            var c = path.Split("]");
            int x = Convert.ToInt32(c[0].Split("[")[1]);
            int i = Convert.ToInt32(c[1].Split("[")[1]);
            try
            {
                RecentMatch match = PlayersViewmodel.PlayerModelsObservable[x].RecentMatchesObservable[i];
                RecentMatch_Popup.ShowPopUp(img, match);
            }
            catch
            {
                return;
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;

            Binding binding = BindingOperations.GetBinding(label, Label.ContentProperty);
            string path = Convert.ToString(binding.Path.Path);

            //"[x].RecentMatchesObservable[i].hero"
            var c = path.Split("]");
            int x = Convert.ToInt32(c[0].Split("[")[1]);
            PlayerModel player = PlayersViewmodel.PlayerModelsObservable[x];

            PlayerPopUp.ShowPopUp(label, player);
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await UpdatePlayers();
        }

        private bool working = false;
        private async Task<PlayerModel[]> UpdatePlayers()
        {
            if (working) return null;
            working = true;
            pbProgress.IsIndeterminate = true;

            CurrentMatch currentMatch = new CurrentMatch();
            PlayerModel[] result = await Task.Run(() => currentMatch.UpdateCurrentMatch());

            for (int i = 0; i < 10; i++)
            {
                PlayersViewmodel.PlayerModelsObservable[i] = result[i];
            }

            pbProgress.IsIndeterminate = false;
            return result;
        }

        /// Test methouds
        private void tst()
        {
            ObservableCollection<PlayerModel> players = new ObservableCollection<PlayerModel>();
            ObservableCollection<RecentMatch> recentMatches = new ObservableCollection<RecentMatch>();

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Resources/Icons/Abaddon.png");
            image.EndInit();

            for (int i = 0; i < 20; i++)
            {
                RecentMatch recent = new RecentMatch()
                {
                    hero = image,
                    match_id = Convert.ToString(i),
                    radiant_win = true,
                    Duration = "60",
                    GameMode = "Turbo",
                    HeroName = "Abadon",
                    Kills = "10",
                    Deaths = "3",
                    Assists = "12",
                    XPpm = "1000",
                    Gpm = "1300",
                    HeroDmg = "50000",
                    TowerDmg = "10000",
                    HeroHeal = "50",
                    LastHit = "120",
                };
                recentMatches.Add(recent);
            }

            for (int i = 0; i < 10; i++)
            {
                PlayerModel pvm = new PlayerModel()
                {
                    Avatar = new BitmapImage(),
                    Name = $"n{i}",
                    RecentMatchesObservable = recentMatches
                };
                players.Add(pvm);
            }
            PlayersViewmodel.PlayerModelsObservable = players;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
