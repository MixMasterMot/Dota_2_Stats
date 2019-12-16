using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Dota_2_Stats.Models;

namespace Dota_2_Stats.PopUps
{
    public class RecentGamesPopUp : BasePopUp
    {

        public Popup GetPopup
        {
            get { return _popUp; }
        }

        public RecentGamesPopUp()
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Colors.Blue);
            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(100);
            grid.ColumnDefinitions.Add(c1);

            ColumnDefinition c2 = new ColumnDefinition();
            c2.Width = new GridLength(80);
            grid.ColumnDefinitions.Add(c2);

            for (int i = 0; i < 10; i++)
            {
                RowDefinition r0 = new RowDefinition();
                r0.Height = new GridLength(30);
                grid.RowDefinitions.Add(r0);
            }

            //duration
            GenerateCell(grid, "Duration", 0, 0);
            GenerateCell(grid, "Duration V", 1, 0);
            //game mode
            GenerateCell(grid, "Game Mode", 0, 1);
            GenerateCell(grid, "Game Mode V", 1, 1);
            //hero name
            GenerateCell(grid, "Hero", 0, 2);
            GenerateCell(grid, "Hero V", 1, 2);
            //kills
            GenerateCell(grid, "K:D:A", 0, 3);
            GenerateCell(grid, "K:D:A V", 1, 3);
            //XPpm
            GenerateCell(grid, "XP/m", 0, 4);
            GenerateCell(grid, "XP/m V", 1, 4);
            //SetGold
            GenerateCell(grid, "G/m", 0, 5);
            GenerateCell(grid, "G/m V", 1, 5);
            //hero damage
            GenerateCell(grid, "Hero Damage", 0, 6);
            GenerateCell(grid, "Hero Damage V", 1, 6);
            //tower damage
            GenerateCell(grid, "Tower Damage", 0, 7);
            GenerateCell(grid, "Tower Damage V", 1, 7);
            //heal
            GenerateCell(grid, "Healing", 0, 8);
            GenerateCell(grid, "Healing V", 1, 8);
            //lasthit
            GenerateCell(grid, "Last Hits", 0, 9);
            GenerateCell(grid, "Last Hits V", 1, 9);

            _popUp.Child = grid;
        }

        public void ShowPopUp(UIElement target, RecentMatch recentMatch)
        {
            //duration
            //SetDuration(0,0);
            GetTextBlock(1, 0).Text = recentMatch.Duration;
            //game made
            GetTextBlock(1, 1).Text = recentMatch.GameMode;
            //hero name
            GetTextBlock(1, 2).Text = recentMatch.HeroName;
            //kills
            GetTextBlock(1, 3).Text = $"{recentMatch.Kills}:{recentMatch.Deaths}:{recentMatch.Assists}";
            //XPpm
            GetTextBlock(1, 4).Text = recentMatch.XPpm;
            //SetGold
            GetTextBlock(1, 5).Text = recentMatch.Gpm;
            //hero damage
            GetTextBlock(1, 6).Text = recentMatch.HeroDmg;
            //tower damage
            GetTextBlock(1, 7).Text = recentMatch.TowerDmg;
            //heal
            GetTextBlock(1, 8).Text = recentMatch.HeroHeal;
            //lasthit
            GetTextBlock(1, 9).Text = recentMatch.LastHit;

            PlacementTarget(target);
        }
        
    }
}
