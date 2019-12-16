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
    public class PlayerPopUp : BasePopUp
    {
        // matches
        // win rate
        // mmr
        // estimate mmr

        public PlayerPopUp()
        {
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Colors.Blue);
            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(100);
            grid.ColumnDefinitions.Add(c1);

            ColumnDefinition c2 = new ColumnDefinition();
            c2.Width = new GridLength(80);
            grid.ColumnDefinitions.Add(c2);

            for (int i = 0; i < 4; i++)
            {
                RowDefinition r0 = new RowDefinition();
                r0.Height = new GridLength(30);
                grid.RowDefinitions.Add(r0);
            }

            //matches
            GenerateCell(grid, "Matches", 0, 0);
            GenerateCell(grid, "Matches V", 1, 0);
            //win rate
            GenerateCell(grid, "Win Rate", 0, 1);
            GenerateCell(grid, "Win Rate V", 1, 1);
            //mmr
            GenerateCell(grid, "MMR", 0, 2);
            GenerateCell(grid, "MMR V", 1, 2);
            //est mmr
            GenerateCell(grid, "Estimate MMR", 0, 3);
            GenerateCell(grid, "Rstimate MMR V", 1, 3);

            _popUp.Child = grid;
        }

        public void ShowPopUp(UIElement target, PlayerModel playerModel)
        {
            //matches
            GetTextBlock(1, 0).Text = playerModel.Matches;
            //win rate
            GetTextBlock(1, 1).Text = playerModel.WinRate;
            //mmr
            GetTextBlock(1, 2).Text = playerModel.MMR;
            //est mmt
            GetTextBlock(1, 3).Text = playerModel.EstMMR;

            PlacementTarget(target);
        }
    }
}
