using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Contracts.Models
{
    public class ConferenceData
    {
        public int Version
        {
            get;
            set;
        }

        public List<Day> Days
        {
            get;
            set;
        }

        public List<Room> Rooms
        {
            get;
            set;
        }

        public List<Speaker> Speakers
        {
            get;
            set;
        }

        public List<Session> Sessions
        {
            get;
            set;
        }

        public List<SessionSpeakerRelation> SessionSpeakerRelations
        {
            get;
            set;
        }

        public List<Slot> Slots
        {
            get;
            set;
        }

        //public List<Event> Events
        //{
        //    get;
        //    set;
        //}

        //public string Test
        //{
        //    get;
        //    set;
        //}

        public string PicturesLocation
        {
            get;
            set;
        }
    }
}
