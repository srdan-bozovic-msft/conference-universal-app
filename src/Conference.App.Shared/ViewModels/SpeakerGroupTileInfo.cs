using Conference.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.ViewModels
{
    public class SpeakerGroupTileInfo : ISpeakerGroupTileInfo
    {
        private static readonly Random Rnd = new Random((int)DateTime.Now.Ticks);
        public SpeakerGroupTileInfo(string groupName)
        {
            GroupName = groupName.ToUpper();
            Speakers = new List<ISpeakerTileInfo>();
        }
        public string GroupName
        {
            get;
            private set;
        }

        public List<ISpeakerTileInfo> Speakers
        {
            get;
            private set;
        }

        public string ImageUrl
        {
            get
            {
                return Speakers[Rnd.Next(Speakers.Count - 1)].ImageUrl;
            }
        }
    
    }
}
