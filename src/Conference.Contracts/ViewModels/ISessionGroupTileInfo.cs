using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Contracts.ViewModels
{
    public interface ISessionGroupTileInfo
    {
        string GroupName { get; }
        List<ISessionTileInfo> Sessions { get; }
    }
}
