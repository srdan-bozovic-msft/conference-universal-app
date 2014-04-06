using Conference.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Contracts.Models
{
    public class Slot
    {
        public int Id
        {
            get;
            set;
        }

        public int TimeLine
        {
            get;
            set;
        }

        public int DayId
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public byte StartHour
        {
            get;
            set;
        }

        public byte StartMinute
        {
            get;
            set;
        }

        public byte EndHour
        {
            get;
            set;
        }

        public byte EndMinute
        {
            get;
            set;
        }
    }
}
