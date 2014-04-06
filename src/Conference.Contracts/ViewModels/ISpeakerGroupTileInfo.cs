using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Contracts.ViewModels
{
    public interface ISpeakerGroupTileInfo
    {
        string GroupName { get; }
        List<ISpeakerTileInfo> Speakers { get; }
    }
}
