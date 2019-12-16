using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Dota_2_Stats.Models
{
    public class RecentMatch : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _match_id { get; set; }
        public string match_id
        {
            get { return _match_id; }
            set
            {
                if (_match_id != value)
                {
                    _match_id = value;
                    NotifyPropertyChanged("match_id");
                }
            }
        }

        private bool _radiant_win = false;
        public bool radiant_win
        {
            get { return _radiant_win; }
            set
            {
                _radiant_win = value;
                NotifyPropertyChanged("hero");
            }
        }

        private BitmapImage _hero;
        public BitmapImage hero
        {
            get { return _hero; }
            set
            {
                _hero = value;
                NotifyPropertyChanged("hero");
            }
        }

        public bool Win { get; set; }
        public SolidColorBrush WinBrush { get; set; }
        public string Duration { get; set; }
        public string GameMode { get; set; }
        public string HeroName { get; set; }
        public string Kills { get; set; }
        public string Deaths { get; set; }
        public string Assists { get; set; }
        public string XPpm { get; set; }
        public string Gpm { get; set; }
        public string HeroDmg { get; set; }
        public string TowerDmg { get; set; }
        public string HeroHeal { get; set; }
        public string LastHit { get; set; }

    }
}
