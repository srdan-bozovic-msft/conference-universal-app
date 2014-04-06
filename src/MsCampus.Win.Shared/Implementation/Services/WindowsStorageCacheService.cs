using MsCampus.Win.Shared.Contracts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MsCampus.Win.Shared.Implementation.Services
{
    public class WindowsStorageCacheService : ICacheService
    {
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                await localFolder.GetFileAsync(key).AsTask().ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }   
        }


        public async Task PutAsync(string key, object value)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var fileTask = localFolder.CreateFileAsync(
                        key, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);
                var json = JsonConvert.SerializeObject(value);
                await FileIO.WriteTextAsync(await fileTask, json).AsTask().ConfigureAwait(false);
            }
            catch
            {
            }
        }

        public async Task<ICacheItem<T>> GetAsync<T>(string key)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.GetFileAsync(key).AsTask().ConfigureAwait(false);
                var json = await FileIO.ReadTextAsync(file).AsTask().ConfigureAwait(false);
                return new CacheItem<T>(JsonConvert.DeserializeObject<T>(json), file.DateCreated.LocalDateTime);
            }
            catch
            {
                return new CacheItem<T>(default(T), DateTime.MinValue);
            }
        }
    }
}
