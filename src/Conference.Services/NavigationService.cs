using MsCampus.Win.Shared.Implementation.Services;
using Conference.Contracts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.Services
{
    public class NavigationService : NavigationServiceBase
    {
        public class PageNames
        {
            public const string SpeakersPageView = "Conference.App.Views.SpeakersPageView";
        }
        public override void Navigate(string type, object parameter)
        {
            switch (type)
            {
                case PageNames.SpeakersPageView:
                    Navigate<IHomePageView>(parameter); break;

            }            
        }
    }
}
