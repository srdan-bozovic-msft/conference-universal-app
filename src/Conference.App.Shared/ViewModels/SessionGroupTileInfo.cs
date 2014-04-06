using Conference.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.ViewModels
{
    public class SessionGroupTileInfo : ISessionGroupTileInfo
    {
        public SessionGroupTileInfo(string groupName)
        {
            GroupName = groupName.ToUpper();
            Sessions = new List<ISessionTileInfo>();
        }
        public string GroupName
        {
            get;
            private set;
        }

        public List<ISessionTileInfo> Sessions
        {
            get;
            private set;
        }    
    }
}
