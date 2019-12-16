using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Media.Imaging;

namespace Dota_2_Stats.Models
{
    public class PlayerModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _Name = "";
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private BitmapImage _Avatar = new BitmapImage();
        public BitmapImage Avatar
        {
            get { return _Avatar; }
            set
            {
                _Avatar = value;
                NotifyPropertyChanged("Avatar");
            }
        }

        private string _AvatarPath = "";
        public string AvatarPath
        {
            get { return _AvatarPath; }
            set
            {
                _AvatarPath = value;
                NotifyPropertyChanged("Avatar");
            }
        }

        public string Matches { get; set; } = "matches V";
        public string WinRate { get; set; } = "winrate V";
        public string MMR { get; set; } = "mmr V";
        public string EstMMR { get; set; } = "est mmr V";

        ObservableCollection<RecentMatch> recentMatchesObservable = new ObservableCollection<RecentMatch>();
        public ObservableCollection<RecentMatch> RecentMatchesObservable
        {
            get { return recentMatchesObservable; }
            set
            {
                recentMatchesObservable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RecentMatchesObservable"));
            }
        }
    }
}
