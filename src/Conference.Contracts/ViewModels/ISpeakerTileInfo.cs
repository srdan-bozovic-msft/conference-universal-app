using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Contracts.ViewModels
{
    public interface ISpeakerTileInfo
    {
        int SpeakerId { get; }
        string SpeakerName { get; }
        string ImageUrl { get; }
    }
}
