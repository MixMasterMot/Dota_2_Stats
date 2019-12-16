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
    public class BasePopUp
    {

        public Popup _popUp = new Popup();

        private void IsOpen(bool open)
        {
            _popUp.IsOpen = open;
        }

        public Border GetBorder(int thickness = 2)
        {
            Border border = new Border()
            {
                BorderBrush = new SolidColorBrush(Colors.Green),
                BorderThickness = new Thickness(thickness),
                Background = new SolidColorBrush(Colors.Purple)
            };
            return border;
        }

        public TextBlock GetTextBlock(string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            return textBlock;
        }

        public TextBlock GetTextBlock(int col, int row)
        {
            Grid g = _popUp.Child as Grid;
            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if (Grid.GetRow(e) == row && Grid.GetColumn(e) == col)
                {
                    var s = e as Border;
                    var t = s.Child as TextBlock;
                    return t;
                }

            }
            return null;
        }

        public void GenerateCell(Grid grid, String Text, int Column, int Row)
        {
            Border bHead = GetBorder();
            bHead.Child = GetTextBlock(Text);

            Grid.SetColumn(bHead, Column);
            Grid.SetRow(bHead, Row);
            grid.Children.Add(bHead);
        }

        public void PlacementTarget(UIElement target)
        {
            if (_popUp.IsOpen == false)
            {
                _popUp.PlacementTarget = target;
                IsOpen(true);
            }
            else if (_popUp.PlacementTarget != null && _popUp.PlacementTarget.Equals(target))
            {
                IsOpen(false);
            }
            else if (!_popUp.PlacementTarget.Equals(target))
            {
                IsOpen(false);
                _popUp.PlacementTarget = target;
                IsOpen(true);
            }
            else
            {
                _popUp.PlacementTarget = target;
                IsOpen(true);
            }
        }
    }
}
