using Dota_2_Stats.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Dota_2_Stats.ViewModels
{
    class PlayersViewmodel : INotifyPropertyChanged
    {
        ObservableCollection<PlayerModel> _PlayerModelsObservable;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<PlayerModel> PlayerModelsObservable
        {
            get { return _PlayerModelsObservable; }
            set
            {
                _PlayerModelsObservable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RecentMatchesObservable"));
            }
        }

        public PlayersViewmodel()
        {
            _PlayerModelsObservable = new ObservableCollection<PlayerModel>();
        }
    }
}
