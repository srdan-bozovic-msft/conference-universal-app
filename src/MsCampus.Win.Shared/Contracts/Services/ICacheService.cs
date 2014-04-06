using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsCampus.Win.Shared.Contracts.Services
{
    public interface ICacheService
    {
        Task<bool> ExistsAsync(string key);
        Task PutAsync(string key, object value);
        Task<ICacheItem<T>> GetAsync<T>(string key);
    }
}
