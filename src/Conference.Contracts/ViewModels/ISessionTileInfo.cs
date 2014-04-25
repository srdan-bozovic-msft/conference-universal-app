using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Contracts.ViewModels
{
    public interface ISessionTileInfo : INotifyPropertyChanged
    {
        int Id { get; }
        string Title { get; }
        string TrackCode { get; }
        string TrackCodeString { get; }
        string TrackTitle { get; }
        string TrackImageUrl { get; }
        string Speakers { get; }
        string Starts { get; }
        string Ends { get; }
        int Level { get; }
        string Language { get; }
        bool IsTrackTwoChars { get; }
        bool IsTrackThreeChars { get; }
        bool IsFavorite { get; set; }
    }
}
