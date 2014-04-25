using Conference.Contracts.Models;
using MsCampus.Win.Shared.Contracts.Repositories;
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
        Task<RepositoryResult<ConferenceData>> GetConferenceDataAsync();
        Task<RepositoryResult<ConferenceData>> GetConferenceDataAsync(CancellationToken cancellationToken);
    }
}
