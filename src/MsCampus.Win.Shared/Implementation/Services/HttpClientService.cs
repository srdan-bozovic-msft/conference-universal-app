using MsCampus.Win.Shared.Contracts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MsCampus.Win.Shared.Implementation.Services
{
    public class HttpClientService : IHttpClientService
    {
        public async Task<T> GetJsonAsync<T>(string url, CancellationToken cancellationToken)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip;
            var client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
            if (response != null &&
                (response.StatusCode == System.Net.HttpStatusCode.OK))
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return await JsonConvert.DeserializeObjectAsync<T>(responseContent).ConfigureAwait(false);
            }
            else
            {
                return default(T);
            }
        }
    }
}
