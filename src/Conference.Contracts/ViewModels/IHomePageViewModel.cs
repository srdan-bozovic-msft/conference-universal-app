using MsCampus.Win.Shared.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Conference.Contracts.ViewModels
{
    public interface IHomePageViewModel : IPageViewModel
    {
        ObservableCollection<ISpeakerGroupTileInfo> SpeakerGroupTileInfos { get; set; }
        ICommand SpeakerSelectedCommand { get; set; }
        ObservableCollection<ISessionGroupTileInfo> SessionGroupTileInfos { get; set; }
        ICommand SessionSelectedCommand { get; set; }

        ICommand ShowMapCommand { get; set; }
        int SelectedSessionId { get; set; }
        int SelectedSpeakerId { get; set; }
        bool IsModal { get; set; }

        bool IsSynchronizing { get; set; }
    }
}
