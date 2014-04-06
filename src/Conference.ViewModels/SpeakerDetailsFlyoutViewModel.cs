using Conference.Contracts.Repositories;
using Conference.Contracts.ViewModels;
using Conference.Contracts.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MsCampus.Win.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Conference.ViewModels
{
    public class SpeakerDetailsFlyoutViewModel : ViewModelBase, ISpeakerDetailsFlyoutViewModel
    {
        private INavigationService _navigationService;
        private IConferenceRepository _conferenceRepository;
        private IHomePageViewModel _homePageViewModel;
        private IFlyoutService _flyoutService;

        private ObservableCollection<ISessionTileInfo> _sessionTileInfos;

        private bool _navigatingToNextFlyout;
        private Type _navigatedFrom;

        public SpeakerDetailsFlyoutViewModel(
            INavigationService navigationService,
            IConferenceRepository conferenceRepository,
            IHomePageViewModel homePageViewModel,
            IFlyoutService flyoutService)
        {
            _navigationService = navigationService;
            _conferenceRepository = conferenceRepository;
            _homePageViewModel = homePageViewModel;
            _flyoutService = flyoutService;

            _sessionTileInfos = new ObservableCollection<ISessionTileInfo>();

            InitializeCommands();
        }

        public ObservableCollection<ISessionTileInfo> SessionTileInfos
        {
            get
            {
                return _sessionTileInfos;
            }
            set
            {
                _sessionTileInfos = value;
                RaisePropertyChanged("SessionTileInfos");
            }
        }

        private void InitializeCommands()
        {
            LoadedCommand = new RelayCommand(
                () =>
                {
                    _homePageViewModel.IsModal = true;
                });

            UnloadedCommand = new RelayCommand(
                () =>
                {
                    if (!_navigatingToNextFlyout)
                    {
                        _homePageViewModel.IsModal = false;
                    }

                    _navigatingToNextFlyout = false;
                });

            BackClickCommand = new RelayCommand(
                () =>
                {

                    if (_navigatedFrom != null && _navigatedFrom != typeof(IHomePageView))
                    {
                        _navigatingToNextFlyout = true;
                        _flyoutService.ShowIndependent<ISessionDetailsFlyoutView>(new Tuple<Type, int>(null, _homePageViewModel.SelectedSessionId));
                    }
                });

            SessionSelectedCommand = new RelayCommand<ISessionTileInfo>(
                sessionTileInfo =>
                {                  
                    SelectedSessionId = sessionTileInfo.Id;
                    _homePageViewModel.SelectedSessionId = sessionTileInfo.Id;
                    _navigatingToNextFlyout = true;
                    _flyoutService.ShowIndependent<ISessionDetailsFlyoutView>(new Tuple<Type, int>(typeof(ISessionDetailsFlyoutView), SelectedSessionId));
                });
        }

        public async void Initialize(object parameter)
        {
            if (parameter != null)
            {
                _navigatedFrom = ((Tuple<Type, int>)parameter).Item1;
                var selectedSpeakerId = ((Tuple<Type, int>)parameter).Item2;
                var data = await _conferenceRepository.GetConferenceDataAsync();
                var speaker = data.Speakers.FirstOrDefault(s => s.Id == selectedSpeakerId);
                var sessionSpeakerRelations = data.SessionSpeakerRelations.Where(s => s.SpeakerId == speaker.Id);

                SpeakerName = String.Format("{0} {1}", speaker.FirstName, speaker.LastName);
                ImageUrl = speaker.PictureUrl;
                Bio = speaker.Bio;
                CompanyString = String.Format("[{0}]", speaker.Company);

                SessionTileInfos.Clear();

                foreach (var sessionSpeakerRelation in sessionSpeakerRelations)
                {
                    SessionTileInfos.Add(new SessionTileInfo(data.Sessions.Where(s => s.Id == sessionSpeakerRelation.SessionId).First(), data));
                }
            }
            else
            {
                _navigatedFrom = null;
            }
        }

        private string _speakerName;
        public string SpeakerName
        {
            get
            {
                return _speakerName;
            }
            set
            {
                if (value != _speakerName)
                {
                    _speakerName = value;
                    RaisePropertyChanged(() => SpeakerName);
                }
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                if (value != _imageUrl)
                {
                    _imageUrl = value;
                    RaisePropertyChanged(() => ImageUrl);
                }
            }
        }

        private string _bio;
        public string Bio
        {
            get
            {
                return _bio;
            }
            set
            {

                    _bio = value;
                    RaisePropertyChanged(() => Bio);

            }
        }

        private string _companyString;
        public string CompanyString
        {
            get
            {
                return _companyString;
            }
            set
            {
                if (value != _companyString)
                {
                    _companyString = value;
                    RaisePropertyChanged(() => CompanyString);
                }
            }
        }

        public ICommand BackClickCommand
        {
            get;
            set;
        }
        public ICommand LoadedCommand
        {
            get;
            set;
        }
        public ICommand UnloadedCommand
        {
            get;
            set;
        }
        public ICommand SessionSelectedCommand
        {
            get;
            set;
        }

        public int SelectedSessionId { get; set; }
    }
}
