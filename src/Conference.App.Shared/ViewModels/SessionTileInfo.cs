using Conference.Contracts.Models;
using Conference.Contracts.Models.Helpers;
using Conference.Contracts.ViewModels;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.ViewModels
{
    public class SessionTileInfo : ViewModelBase, ISessionTileInfo
    {
        public SessionTileInfo(Session session, ConferenceData conferenceData)
        {
            Id = session.Id;
            Title = session.Title;
            TrackCode = session.Track.TrimEnd();
            TrackCodeString = String.Format("[{0}]", TrackCode);
            TrackTitle = TrackHelper.GetTitleForTrack(TrackCode);
            TrackImageUrl = TrackHelper.GetImageUrlForTrack(TrackCode);
            Language = session.Lang;
            Level = session.Level;

            IsTrackThreeChars = session.Track.TrimEnd().Length == 3;
            IsTrackTwoChars = session.Track.TrimEnd().Length == 2;
            IsFavorite = session.IsFavorite;

            var speakers = new List<string>();
            foreach (var relation in conferenceData.SessionSpeakerRelations.Where(r=>r.SessionId == session.Id))
            {
                var speaker = conferenceData.Speakers.First(s=>s.Id == relation.SpeakerId);
                speakers.Add(string.Format("{0} {1}",speaker.FirstName,speaker.LastName));
            }
            Speakers = string.Join("; ",speakers);
            var slot = conferenceData.Slots.First(s => s.TimeLine == session.TimeLine);
            Starts = string.Format("{0:00}:{1:00}", slot.StartHour, slot.StartMinute);
            Ends = string.Format("{0:00}:{1:00}", slot.EndHour, slot.EndMinute);
        }

        public int Id
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public string TrackCode
        {
            get;
            private set;
        }

        public string TrackCodeString
        {
            get;
            private set;
        }

        public string TrackTitle
        {
            get;
            private set;
        }

        public string TrackImageUrl
        {
            get;
            private set;
        }

        public string Speakers
        {
            get;
            private set;
        }

        public string Starts
        {
            get;
            private set;
        }

        public string Ends
        {
            get;
            private set;
        }

        public int Level
        {
            get;
            private set;
        }

        public string Language
        {
            get;
            private set;
        }

        public bool IsTrackThreeChars
        {
            get;
            private set;
        }

        public bool IsTrackTwoChars
        {
            get;
            private set;
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get
            {
                return _isFavorite;
            }
            set
            {
                if(value != _isFavorite)
                {
                    _isFavorite = value;
                    RaisePropertyChanged(() => IsFavorite);
                }
            }
        }

    }
}
