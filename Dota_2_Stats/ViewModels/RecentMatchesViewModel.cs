using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Dota_2_Stats.Models;

namespace Dota_2_Stats.ViewModels
{
    class RecentMatchesViewModel : INotifyPropertyChanged
    {
        ObservableCollection<RecentMatch> recentMatchesObservable;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<RecentMatch> RecentMatchesObservable
        {
            get { return recentMatchesObservable; }
            set
            {
                recentMatchesObservable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RecentMatchesObservable"));
            }
        }

        public RecentMatchesViewModel()
        {
            RecentMatchesObservable = new ObservableCollection<RecentMatch>();
        }
    }
}
