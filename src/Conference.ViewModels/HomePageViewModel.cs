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

namespace Conference.ViewModels
{
    public class HomePageViewModel : TabPageViewModel, IHomePageViewModel
    {

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
                _flyoutService.ShowIndependent<ISpeakerDetailsFlyoutView>(new Tuple<Type, int>(typeof(IHomePageView), SelectedSpeakerId));
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
            IsSynchronizing = true;
            var conferenceData = await _conferenceRepository.GetConferenceDataAsync();
            IsSynchronizing = false;
            SessionGroupTileInfos = GroupSessions(conferenceData);
            SpeakerGroupTileInfos = GroupSpeakers(conferenceData.Speakers);
        }

        private static ObservableCollection<ISessionGroupTileInfo> GroupSessions(ConferenceData conferenceData)
        {
            var sessionGroupTileInfoList = new List<SessionGroupTileInfo>();
            var groupHeaders = conferenceData.Slots.OrderBy(s=>s.StartHour*100+s.StartMinute).Select(s => string.Format("{0:00}:{1:00}", s.StartHour, s.StartMinute)).Distinct().ToArray();
            var sessionInfos = conferenceData.Sessions.Select(s => new SessionTileInfo(s, conferenceData)).OrderBy(s=>s.Starts);
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
