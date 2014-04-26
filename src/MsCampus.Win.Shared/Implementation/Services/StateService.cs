using MsCampus.Win.Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MsCampus.Win.Shared.Implementation.Services
{
    public class StateService : IStateService
    {
        private ApplicationDataContainer _localSettings = null;
        public string Parameter { get; set; }
        public string ViewName { get; set; }

        public StateService()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public void AddState(string viewName, string parameter)
        {
            ViewName = viewName;
            Parameter = parameter;
        }

        public void SaveState()
        {
            _localSettings.Values["viewName"] = ViewName;
            _localSettings.Values["parameter"] = Parameter;
        }

        public void LoadState()
        {
            ViewName = (_localSettings.Values["viewName"]??"").ToString();
            Parameter = (_localSettings.Values["parameter"]??"").ToString();
        }
    }
}
