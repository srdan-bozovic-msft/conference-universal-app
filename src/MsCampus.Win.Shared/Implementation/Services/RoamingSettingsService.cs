using MsCampus.Win.Shared.Contracts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace MsCampus.Win.Shared.Implementation.Services
{
    public class RoamingSettingsService : IRoamingSettingsService
    {
        private static IPropertySet Settings = ApplicationData.Current.RoamingSettings.Values;

        public event EventHandler DataChanged;

        public RoamingSettingsService()
        {
            ApplicationData.Current.DataChanged += Current_DataChanged;
        }

        void Current_DataChanged(ApplicationData sender, object args)
        {
            if (DataChanged != null)
            {
                DataChanged(this, EventArgs.Empty);
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return Settings.ContainsKey(key);
        }

        public async Task PutAsync(string key, object value)
        {
            if (Settings.ContainsKey(key))
            {
                Settings[key] = await JsonConvert.SerializeObjectAsync(value).ConfigureAwait(false);
            }
            else
            {
                Settings.Add(key, await JsonConvert.SerializeObjectAsync(value).ConfigureAwait(false));
            }
            ApplicationData.Current.SignalDataChanged();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if(Settings.ContainsKey(key))
            {
                return await JsonConvert.DeserializeObjectAsync<T>((string)Settings[key]).ConfigureAwait(false);
            }
            return default(T);
        }
    }
}
