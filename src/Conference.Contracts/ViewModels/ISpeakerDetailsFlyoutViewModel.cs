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
    public interface ISpeakerDetailsFlyoutViewModel : IPageViewModel
    {
        string SpeakerName { get; set; }
        string ImageUrl { get; set; }
        string Bio { get; set; }
        string CompanyString { get; set; }     
        ObservableCollection<ISessionTileInfo> SessionTileInfos { get; set; }
        ICommand SessionSelectedCommand { get; set; }
        ICommand LoadedCommand { get; set; }
        ICommand UnloadedCommand { get; set; }
        ICommand BackClickCommand { get; set; }
        int SelectedSessionId { get; set; }      
    }
}
