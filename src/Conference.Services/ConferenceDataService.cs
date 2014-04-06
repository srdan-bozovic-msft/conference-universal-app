using MsCampus.Win.Shared.Contracts.Services;
using Conference.Contracts.Models;
using Conference.Contracts.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.Services
{
    public class ConferenceDataService : IConferenceDataService
    {
        private const string ServiceUriString = "http://tarabica.msforge.net/Device/";

        private IHttpClientService _httpClientService;
        public ConferenceDataService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<ConferenceData> GetConfDataAsync(CancellationToken cancellationToken)
        {
            var data = await _httpClientService.GetJsonAsync<ConferenceData>(ServiceUriString + "GetData", cancellationToken).ConfigureAwait(false);
            foreach (var speaker in data.Speakers)
            {
                speaker.PictureUrl = string.Format("{0}/{1}", data.PicturesLocation, speaker.PictureUrl);
            }
            return data;
        }

        public async Task<int> GetVersionAsync(CancellationToken cancellationToken)
        {
            return await _httpClientService.GetJsonAsync<int>(ServiceUriString + "GetVersion", cancellationToken).ConfigureAwait(false);
        }
    }
}
