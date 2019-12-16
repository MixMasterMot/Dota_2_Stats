using Dota_2_Stats.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Dota_2_Stats.Converters
{
    public class ToWinBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool? Win = value as bool?;
                if (Win.Value)
                {
                    SolidColorBrush brush = new SolidColorBrush(Colors.Green);
                    return brush;
                }
                SolidColorBrush brushLos = new SolidColorBrush(Colors.Red);
                return brushLos;
            }
            catch
            {
                SolidColorBrush brushLos = new SolidColorBrush(Colors.Red);
                return brushLos;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
