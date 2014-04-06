using Conference.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.Contracts.Repositories
{
    public interface IConferenceRepository
    {
        Task<ConferenceData> GetConferenceDataAsync();
        Task<ConferenceData> GetConferenceDataAsync(CancellationToken cancellationToken);
    }
}
