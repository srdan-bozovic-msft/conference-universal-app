using Conference.Contracts.Models;
using Conference.Contracts.Models.Helpers;
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
    public class SessionDetailsFlyoutViewModel : ViewModelBase, ISessionDetailsFlyoutViewModel
    {
        private INavigationService _navigationService;
        private IConferenceRepository _conferenceRepository;
        private IHomePageViewModel _homePageViewModel;
        private IFlyoutService _flyoutService;

        private ObservableCollection<ISpeakerTileInfo> _speakerTileInfos;

        private bool _navigatingToNextFlyout;
        private Type _navigatedFrom;

        public SessionDetailsFlyoutViewModel(
            INavigationService navigationService,
            IConferenceRepository conferenceRepository,
            IHomePageViewModel homePageViewModel,
            IFlyoutService flyoutService)
        {
            _navigationService = navigationService;
            _conferenceRepository = conferenceRepository;
            _homePageViewModel = homePageViewModel;
            _flyoutService = flyoutService;

            _speakerTileInfos = new ObservableCollection<ISpeakerTileInfo>();

            InitializeCommands();
        }

        public ObservableCollection<ISpeakerTileInfo> SpeakerTileInfos
        {
            get
            {
                return _speakerTileInfos;
            }
            set
            {
                _speakerTileInfos = value;
                RaisePropertyChanged("SpeakerTileInfos");
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
                        _flyoutService.ShowIndependent<ISpeakerDetailsFlyoutView>(new Tuple<Type, int>(null, _homePageViewModel.SelectedSpeakerId));
                    }
                });

            SpeakerSelectedCommand = new RelayCommand<ISpeakerTileInfo>(
                speakerTileInfo =>
                {
                    SelectedSpeakerId = speakerTileInfo.SpeakerId;
                    _homePageViewModel.SelectedSpeakerId = speakerTileInfo.SpeakerId;
                    _navigatingToNextFlyout = true;
                    _flyoutService.ShowIndependent<ISpeakerDetailsFlyoutView>(new Tuple<Type, int>(typeof(ISessionDetailsFlyoutView), SelectedSpeakerId));
                });

            SwitchFavoriteCommand = new RelayCommand(
                async () =>
                {
                    var value = !IsFavorite;
                    await _conferenceRepository.UpdateFavoriteStatusAsync(_model.Id, value);
                    IsFavorite = value;
                    foreach (var sessionGroupTileInfo in _homePageViewModel.SessionGroupTileInfos)
                    {
                        foreach (var session in sessionGroupTileInfo.Sessions)
                        {
                            if(session.Id == _model.Id)
                            {
                                session.IsFavorite = IsFavorite;
                            }
                        }
                    }
                });
        }
        public string FavoriteText
        {
            get
            {
                return IsFavorite ? "UnFavorite" : "Favorite";
            }
        }       

        private string _trackString;
        public string TrackString
        {
            get
            {
                return _trackString;
            }
            set
            {
                if (value != _trackString)
                {
                    _trackString = value;
                    RaisePropertyChanged(() => TrackString);
                }
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {

                    _description = value;
                    RaisePropertyChanged(() => Description);

            }
        }

        private string _language;
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                if (value != _language)
                {
                    _language = value;
                    RaisePropertyChanged(() => Language);
                }
            }
        }

        private string _timeString;
        public string TimeString
        {
            get
            {
                return _timeString;
            }
            set
            {
                if (value != _timeString)
                {
                    _timeString = value;
                    RaisePropertyChanged(() => TimeString);
                }
            }
        }

        private string _trackImageUrl;
        public string TrackImageUrl
        {
            get
            {
                return _trackImageUrl;
            }
            set
            {
                if (value != _trackImageUrl)
                {
                    _trackImageUrl = value;
                    RaisePropertyChanged(() => TrackImageUrl);
                }
            }
        }

        private int _level;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value != _level)
                {
                    _level = value;
                    RaisePropertyChanged(() => Level);
                }
            }
        }

        private Room _room;

        public Room Room
        {
            get
            {
                return _room;
            }
            set
            {
                if (value != _room)
                {
                    _room = value;
                    RaisePropertyChanged(() => Room);
                }
            }
        }

        public bool IsFavorite
        {
            get
            {
                return _model != null && _model.IsFavorite;
            }
            set
            {
                if (value != _model.IsFavorite)
                {
                    _model.IsFavorite = value;
                }
                RaisePropertyChanged(() => IsFavorite);
                RaisePropertyChanged(() => FavoriteText);
            }
        }

        private Session _model;

        public async void Initialize(object parameter)
        {
            if (parameter != null)
            {
                _navigatedFrom = ((Tuple<Type, int>)parameter).Item1;
                var selectedSessionId = ((Tuple<Type, int>)parameter).Item2;
                var data = await _conferenceRepository.GetConferenceDataAsync();
                _model = data.Value.Sessions.FirstOrDefault(s => s.Id == selectedSessionId);
                var sessionSpeakerRelations = data.Value.SessionSpeakerRelations.Where(s => s.SessionId == _model.Id);
                var slot = data.Value.Slots.First(s => s.TimeLine == _model.TimeLine);

                TrackString = String.Format("[{0}]", TrackHelper.GetTitleForTrack(_model.Track.TrimEnd()));
                TrackImageUrl = TrackHelper.GetImageUrlForTrack(_model.Track.TrimEnd());
                Title = _model.Title;
                Description = _model.Description;
                Language = _model.Lang;
                Level = _model.Level;
                Room = data.Value.Rooms.Where(r => r.Id == _model.RoomId).First();
                IsFavorite = _model.IsFavorite;
                TimeString = String.Format("{0:00}:{1:00} - {2:00}:{3:00}", slot.StartHour, slot.StartMinute, slot.EndHour, slot.EndMinute);
                SpeakerTileInfos.Clear();
                foreach (var sessionSpeakerRelation in sessionSpeakerRelations)
                {
                    var speaker = data.Value.Speakers.Where(s => s.Id == sessionSpeakerRelation.SpeakerId).First();
                    if(!data.IsCurrent)
                    {
                        speaker.PictureUrl = "/Data/SpeakerPhotos" + speaker.PictureUrl.Substring(speaker.PictureUrl.LastIndexOf('/'));
                    }

                    SpeakerTileInfos.Add(new SpeakerTileInfo(speaker));
                }
            }
            else
            {
                _navigatedFrom = null;
            }
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

        public ICommand BackClickCommand
        {
            get;
            set;
        }
        public ICommand SpeakerSelectedCommand
        {
            get;
            set;
        }

        public ICommand SwitchFavoriteCommand
        {
            get;
            set;
        }
        public int SelectedSpeakerId { get; set; }
    }
}
