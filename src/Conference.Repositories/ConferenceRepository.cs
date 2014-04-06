using MsCampus.Win.Shared.Contracts.Services;
using Conference.Contracts.Models;
using Conference.Contracts.Repositories;
using Conference.Contracts.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.Repositories
{
    public class ConferenceRepository : IConferenceRepository
    {
        private const string ConferenceDataKey = "conferenceData";

        private IConferenceDataService _conferenceDataService;
        private ICacheService _cacheService;

        public ConferenceRepository(
            IConferenceDataService conferenceDataService,
            ICacheService cacheService)
        {
            _conferenceDataService = conferenceDataService;
            _cacheService = cacheService;
        }

        public async Task<ConferenceData> GetConferenceDataAsync()
        {
            var cts = new CancellationTokenSource();
            return await GetConferenceDataAsync(cts.Token);
        }

        public async Task<ConferenceData> GetConferenceDataAsync(CancellationToken cancellationToken)
        {
            var item = await _cacheService.GetAsync<ConferenceData>(ConferenceDataKey).ConfigureAwait(false);
            if (item.HasValue)
            {
                if (cancellationToken.IsCancellationRequested)
                    return item.Value;
                var versionId = item.Value.Version;
                var latestVersionId = await _conferenceDataService.GetVersionAsync(cancellationToken).ConfigureAwait(false);
                if (versionId >= latestVersionId)
                {
                    return item.Value;
                }
            }
            var data = await _conferenceDataService.GetConfDataAsync(cancellationToken).ConfigureAwait(false);
            await _cacheService.PutAsync(ConferenceDataKey, data).ConfigureAwait(false);
            return data;
        }
    }
}
