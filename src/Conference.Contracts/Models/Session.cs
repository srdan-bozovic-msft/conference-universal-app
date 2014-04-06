using Conference.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Contracts.Models
{
    public class Session
    {
        public int Id
        {
            get;
            set;
        }

        public string Track
        {
            get;
            set;
        }

        public int DayId
        {
            get;
            set;
        }

        public int TimeLine
        {
            get;
            set;
        }

        public string Lang
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public int RoomId
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
