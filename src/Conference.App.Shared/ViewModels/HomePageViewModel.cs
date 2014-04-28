using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MsCampus.Win.Shared.Contracts.Services;
using Conference.Contracts.Models;
using Conference.Contracts.Repositories;
using Conference.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conference.Contracts.Views;
using System.Net;
using System.Windows.Input;
using Windows.UI.Xaml;
#if WINDOWS_PHONE_APP
using Conference.PhoneApp.Views;
#endif

namespace Conference.ViewModels
{
    public class HomePageViewModel : TabPageViewModel, IHomePageViewModel
    {
        private ConferenceData _conferenceData;
        private ObservableCollection<ISessionGroupTileInfo> _sessionGroupTileInfos;
        public ObservableCollection<ISessionGroupTileInfo> SessionGroupTileInfos
        {
            get
            {
                return _sessionGroupTileInfos;
            }
            set
            {
                _sessionGroupTileInfos = value;
                RaisePropertyChanged("SessionGroupTileInfos");
            }
        }

        private ObservableCollection<ISessionGroupTileInfo> _allSessionGroupTileInfos;
        public ObservableCollection<ISessionGroupTileInfo> AllSessionGroupTileInfos
        {
            get
            {
                return _allSessionGroupTileInfos;
            }
            set
            {
                _allSessionGroupTileInfos = value;
                RaisePropertyChanged("AllSessionGroupTileInfos");
            }
        }

        private ObservableCollection<ISessionGroupTileInfo> _favoriteSessionGroupTileInfos;
        public ObservableCollection<ISessionGroupTileInfo> FavoriteSessionGroupTileInfos
        {
            get
            {
                return _favoriteSessionGroupTileInfos;
            }
            set
            {
                _favoriteSessionGroupTileInfos = value;
                RaisePropertyChanged("FavoriteSessionGroupTileInfos");
            }
        }

        public ICommand SessionSelectedCommand
        {
            get;
            set;
        }

        private ObservableCollection<ISpeakerGroupTileInfo> _speakerGroupTileInfos;
        public ObservableCollection<ISpeakerGroupTileInfo> SpeakerGroupTileInfos
        {
            get
            {
                return _speakerGroupTileInfos;
            }
            set
            {
                _speakerGroupTileInfos = value;
                RaisePropertyChanged("SpeakerGroupTileInfos");
            }
        }

        public ICommand SpeakerSelectedCommand
        {
            get;
            set;
        }

        public ICommand FavoritesToggledCommand
        {
            get;
            set;
        }

        public ICommand ShowMapCommand
        {
            get;
            set;
        }

        private bool _isModal;
        public bool IsModal
        {
            get
            {
                return _isModal;
            }
            set
            {
                _isModal = value;
                RaisePropertyChanged("IsModal");
            }
        }

        private bool _isSynchronizing;
        public bool IsSynchronizing
        {
            get
            {
                return _isSynchronizing;
            }
            set
            {
                _isSynchronizing = value;
                RaisePropertyChanged("IsSynchronizing");
            }
        }

        private bool _isFavoritesMode;
        public bool IsFavoritesMode
        {
            get
            {
                return _isFavoritesMode;
            }
            set
            {
                _isFavoritesMode = value;
                RaisePropertyChanged("IsFavoritesMode");

                SessionGroupTileInfos = _isFavoritesMode ? FavoriteSessionGroupTileInfos : AllSessionGroupTileInfos;
            }
        }

        private bool _isOffline;
        public bool IsOffline
        {
            get
            {
                return _isOffline;
            }
            set
            {
                _isOffline = value;
                RaisePropertyChanged("IsOffline");
            }
        }

        private INavigationService _navigationService;
        private IConferenceRepository _conferenceRepository;
        private IToastService _toastService;
        private IFlyoutService _flyoutService;

        public HomePageViewModel(
            INavigationService navigationService,
            IConferenceRepository conferenceRepository,
            IFlyoutService flyoutService,
            IToastService toastService)
        {
            _navigationService = navigationService;
            _conferenceRepository = conferenceRepository;
            _flyoutService = flyoutService;
            _toastService = toastService;

            _sessionGroupTileInfos = new ObservableCollection<ISessionGroupTileInfo>();
            _speakerGroupTileInfos = new ObservableCollection<ISpeakerGroupTileInfo>();

            InitializeTabs("Agenda", "Predavači");

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SessionSelectedCommand = new RelayCommand<ISessionTileInfo>((sessionTileInfo) =>
            {
                //_toastService.SendSimpleTextToast("//TODO\nNavigate to session " + sessionTileInfo.Title);
                SelectedSessionId = sessionTileInfo.Id;
                _flyoutService.ShowIndependent<ISessionDetailsFlyoutView>(new Tuple<Type, int>(typeof(IHomePageView), SelectedSessionId));

            });

            SpeakerSelectedCommand = new RelayCommand<ISpeakerTileInfo>((speakerTileInfo) =>
            {
                //_navigationService.Navigate("SpeakerDetails", speakerTileInfo.SpeakerId);
                //_toastService.SendSimpleTextToast("//TODO\nNavigate to speaker " + speakerTileInfo.SpeakerName);
                SelectedSpeakerId = speakerTileInfo.SpeakerId;
#if WINDOWS_PHONE_APP
                _navigationService.Navigate(typeof(SpeakerDetailsPageView), new Tuple<Type, int>(typeof(IHomePageView), SelectedSpeakerId));
#else
                _flyoutService.ShowIndependent<ISpeakerDetailsFlyoutView>(new Tuple<Type, int>(typeof(IHomePageView), SelectedSpeakerId));
#endif
            });

            ShowMapCommand = new RelayCommand(
                async () =>
                {
                    //var longitude = 44.758344;
                    //var latitude = 20.497766;
                    var searchTerm = WebUtility.UrlEncode("Kumodraška 261, Beograd");
                    var uri = new Uri(String.Format("bingmaps:?where={0}", searchTerm));
                    await Windows.System.Launcher.LaunchUriAsync(uri);
                });
        }

        public async void Initialize(object parameter)
        {
            IsOffline = false;
            IsSynchronizing = true;            
            var res = await _conferenceRepository.GetConferenceDataAsync();
            _conferenceData = res.Value; 

            if(!res.IsCurrent)
            {
                foreach (var speaker in res.Value.Speakers)
                {
                    speaker.PictureUrl = "/Data/SpeakerPhotos" + speaker.PictureUrl.Substring(speaker.PictureUrl.LastIndexOf('/'));
                }
            }
            IsSynchronizing = false;
            IsOffline = !res.IsCurrent;

            refreshSessions();
            SpeakerGroupTileInfos = GroupSpeakers(res.Value.Speakers);
        }

        private void refreshSessions()
        {
            AllSessionGroupTileInfos = GroupSessions(_conferenceData);
            FavoriteSessionGroupTileInfos = GroupSessions(_conferenceData, true);

            SessionGroupTileInfos = IsFavoritesMode ? FavoriteSessionGroupTileInfos : AllSessionGroupTileInfos;
        }

        public void UpdateFavoriteSession(int sessionId)
        {
            foreach (var session in _conferenceData.Sessions)
            {
                if (session.Id == sessionId)
                    session.IsFavorite = !session.IsFavorite;
            }

            refreshSessions();
        }
        private static ObservableCollection<ISessionGroupTileInfo> GroupSessions(ConferenceData conferenceData, bool isFavoritesMode = false)
        {
            var sessionGroupTileInfoList = new List<SessionGroupTileInfo>();
            var groupHeaders = conferenceData.Slots.OrderBy(s=>s.StartHour*100+s.StartMinute).Select(s => string.Format("{0:00}:{1:00}", s.StartHour, s.StartMinute)).Distinct().ToArray();
            var sessionInfos = 
                isFavoritesMode ?
                conferenceData.Sessions.Where(s => s.IsFavorite == true).Select(s => new SessionTileInfo(s, conferenceData)).OrderBy(s=>s.Starts) :
                conferenceData.Sessions.Select(s => new SessionTileInfo(s, conferenceData)).OrderBy(s=>s.Starts);
            var groups = new Dictionary<string, SessionGroupTileInfo>();

            foreach (string header in groupHeaders)
            {
                var group = new SessionGroupTileInfo(header);
                sessionGroupTileInfoList.Add(group);
                groups[header] = group;
            }

            foreach (var sessionInfo in sessionInfos)
            {
                var groupName = sessionInfo.Starts;
                groups[groupName].Sessions.Add(sessionInfo);
            }

            var sessionGroupTileInfos = new ObservableCollection<ISessionGroupTileInfo>();
            foreach (var sessionGroup in sessionGroupTileInfoList)
            {
                if (sessionGroup.Sessions.Count > 0)
                {
                    sessionGroupTileInfos.Add(sessionGroup);
                }
            }
            return sessionGroupTileInfos;
        }

        private static ObservableCollection<ISpeakerGroupTileInfo> GroupSpeakers(List<Speaker> speakers)
        {
            var speakerGroupTileInfoList = new List<SpeakerGroupTileInfo>();
            var groupHeaders = new string[] { "a", "b", "c", "č", "ć", "d", "đ", "e", "f", "g", "h", "i", "j", "k", "l", "lj", "m", "n", "nj", "o", "p", "q", "r", "s", "š", "t", "u", "v", "w", "x", "y", "z", "ž" };
            var speakerInfos = speakers.OrderBy(s => s.LastName).Select(s => new SpeakerTileInfo(s));
            var groups = new Dictionary<string, SpeakerGroupTileInfo>();

            foreach (string header in groupHeaders)
            {
                var group = new SpeakerGroupTileInfo(header);
                speakerGroupTileInfoList.Add(group);
                groups[header] = group;
            }

            foreach (var speakerInfo in speakerInfos)
            {
                var lastName = speakerInfo.SpeakerName.Trim().ToLower();
                var groupName = lastName.Substring(0, 2);
                if (groupName == "lj" || groupName == "nj")
                {
                    groups[groupName].Speakers.Add(speakerInfo);
                }
                else
                {
                    groups[lastName.Substring(0, 1)].Speakers.Add(speakerInfo);
                }
            }

            var speakerGroupTileInfos = new ObservableCollection<ISpeakerGroupTileInfo>();
            foreach (var speakerGroup in speakerGroupTileInfoList)
            {
                if (speakerGroup.Speakers.Count > 0)
                {
                    speakerGroupTileInfos.Add(speakerGroup);
                }
            }
            return speakerGroupTileInfos;
        }


        public int SelectedSessionId { get; set; }
        public int SelectedSpeakerId { get; set; }
    }
}
