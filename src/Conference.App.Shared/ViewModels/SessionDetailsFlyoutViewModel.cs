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
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
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
        private int _selectedSessionId;
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

            registerShare();
            InitializeCommands();
        }

        private void registerShare()
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(
                async (sender, e) =>
                {
                    DataRequest request = e.Request;
                    // The Title is mandatory
                    request.Data.Properties.Title = String.Format("[Tarabica 14] {0}", Title);
                    request.Data.Properties.Description = Title;
                    request.Data.SetWebLink(new Uri(String.Format("http://tarabica.msforge.net/Session/Details/{0}", _selectedSessionId)));

                    StringBuilder html = new StringBuilder();
                    html.Append("<div style=\"font-family:segoe ui; background-color:#67b8de; color:#ffffff;\"><div style=\"margin:10px;\"><h3>")
                        .AppendFormat("Posetite predavanje {0} {1} na Tarabici!", Title, TrackString).Append("</h3>")
                        .Append(Description)
                        .Append("<h4><ul>");

                    foreach (var speaker in _speakerTileInfos)
                    {
                        html.AppendFormat("<li>{0} ({1})</li>", speaker.SpeakerName, String.Format("<a href=\"http://tarabica.msforge.net/Speaker/Details/{0}\">Biografija</a>", speaker.SpeakerId));
                    }

                    html.AppendFormat("</ul></h4><h5>Vreme: {0} (5. april 2014.)</br>Sala: {1} (Univerzitet Singidunum, Kumodraška 261)</br></br>Više informacija na: <a href=\"http://tarabica.msforge.net\">http://tarabica.msforge.net</a></h5></div></div>", TimeString, Room.Code);

                    request.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(html.ToString()));

                    DataRequestDeferral deferral = request.GetDeferral();

                    try
                    {
                        StorageFile thumbnailFile =
                            await Package.Current.InstalledLocation.GetFileAsync("Assets\\SquareLogo30x30.scale-100.png");
                        request.Data.Properties.Thumbnail =
                            RandomAccessStreamReference.CreateFromFile(thumbnailFile);
                        StorageFile imageFile =
                            await Package.Current.InstalledLocation.GetFileAsync("Assets\\SquareLogo70x70.scale-100.png");
                        request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
                    }
                    finally
                    {
                        deferral.Complete();
                    }
                });
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
                    if (!_navigatingToNextFlyout && !ShareOpened)
                    {
                        _homePageViewModel.IsModal = false;
                    }

                    if (!ShareOpened)
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
                    IsOpen = false;
                });

            SwitchFavoriteCommand = new RelayCommand(
                async () =>
                {
                    var value = !IsFavorite;
                    await _conferenceRepository.UpdateFavoriteStatusAsync(_model.Id, value);
                    IsFavorite = value;    

                    _homePageViewModel.UpdateFavoriteSession(_model.Id);
                });

            ShareCommand = new RelayCommand(
                () =>
                {
                    _navigatingToNextFlyout = true;
                    ShareOpened = true;
                    IsLightDismissedEnabled = false;
                    Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
                });
        }

        public string FavoriteText
        {
            get
            {
                return IsFavorite ? "Ukloni" : "Odaberi";
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

        private bool _shareOpened;
        public bool ShareOpened
        {
            get
            {
                return _shareOpened;
            }
            set
            {
                _shareOpened = value;
                RaisePropertyChanged("ShareOpened");
            }
        }

        private bool _isLightDismissedEnabled = true;
        public bool IsLightDismissedEnabled
        {
            get
            {
                return _isLightDismissedEnabled;
            }
            set
            {
                if (value != _isLightDismissedEnabled)
                {
                    _isLightDismissedEnabled = value;
                    RaisePropertyChanged(() => IsLightDismissedEnabled);
                }
            }
        }

        private bool _isOpen = true;
        public bool IsOpen
        {
            get
            {
                return _isOpen;
            }
            set
            {
                if (value != _isOpen)
                {
                    _isOpen = value;
                    RaisePropertyChanged(() => IsOpen);
                }
            }
        }

        private Session _model;

        public async void Initialize(object parameter)
        {
            if (parameter != null)
            {
                _navigatedFrom = ((Tuple<Type, int>)parameter).Item1;
                _selectedSessionId = ((Tuple<Type, int>)parameter).Item2;
                var data = await _conferenceRepository.GetConferenceDataAsync();
                _model = data.Value.Sessions.FirstOrDefault(s => s.Id == _selectedSessionId);
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

        public ICommand ShareCommand
        {
            get;
            set;
        }
        public int SelectedSpeakerId { get; set; }
    }
}
