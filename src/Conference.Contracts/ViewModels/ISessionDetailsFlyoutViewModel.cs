using Conference.Contracts.Models;
using MsCampus.Win.Shared.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Conference.Contracts.ViewModels
{
    public interface ISessionDetailsFlyoutViewModel : IPageViewModel
    {
        string TrackString { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Language { get; set; }
        string TimeString { get; set; }
        int Level { get; set; }
        string TrackImageUrl { get; set; }
        Room Room { get; set; }
        bool IsFavorite { get; set; }
        ObservableCollection<ISpeakerTileInfo> SpeakerTileInfos { get; set; }
        ICommand SpeakerSelectedCommand { get; set; }
        ICommand LoadedCommand { get; set; }
        ICommand UnloadedCommand { get; set; }
        ICommand BackClickCommand { get; set; }
        ICommand SwitchFavoriteCommand { get; set; }

        int SelectedSpeakerId { get; set; }
    }
}
