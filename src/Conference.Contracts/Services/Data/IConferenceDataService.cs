using Conference.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.Contracts.Services.Data
{
    public interface IConferenceDataService
    {
        Task<ConferenceData> GetConfDataAsync(CancellationToken cancellationToken);
        Task<int> GetVersionAsync(CancellationToken cancellationToken);
    }
}
