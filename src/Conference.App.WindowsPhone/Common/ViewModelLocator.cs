using MsCampus.Win.Shared.Contracts.Services;
using MsCampus.Win.Shared.DI;
using MsCampus.Win.Shared.Implementation.Services;
using Conference.PhoneApp.Views;
using Conference.Contracts.Repositories;
using Conference.Contracts.Services.Data;
using Conference.Contracts.ViewModels;
using Conference.Contracts.Views;
using Conference.Repositories;
using Conference.Services;
using Conference.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conference.PhoneApp.Common
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            //ViewModel registration
            InstanceFactory.RegisterType<IHomePageViewModel, HomePageViewModel>();
//            InstanceFactory.RegisterType<ISessionDetailsFlyoutViewModel, SessionDetailsFlyoutViewModel>();
            InstanceFactory.RegisterType<ISpeakerDetailsFlyoutViewModel, SpeakerDetailsPageViewModel>();
            InstanceFactory.RegisterType<IAboutAppSettingsFlyoutViewModel, AboutAppSettingsFlyoutViewModel>();
            InstanceFactory.RegisterType<IPrivacySettingsFlyoutViewModel, PrivacySettingsFlyoutViewModel>();

            //View registration
            InstanceFactory.RegisterType<IHomePageView, HomePageView>();
            //InstanceFactory.RegisterType<ISessionDetailsFlyoutView, SessionDetailsFlyoutView>();
            InstanceFactory.RegisterType<ISpeakerDetailsFlyoutView, SpeakerDetailsPageView>();
            //InstanceFactory.RegisterType<IAboutAppSettingsFlyoutView, AboutAppSettingsFlyoutView>();
            //InstanceFactory.RegisterType<IPrivacySettingsFlyoutView, PrivacySettingsFlyoutView>();

            //Repositories registration
            InstanceFactory.RegisterType<IConferenceRepository, ConferenceRepository>();

            //Services registration
            InstanceFactory.RegisterType<IHttpClientService, HttpClientService>();
            InstanceFactory.RegisterType<IConferenceDataService, ConferenceDataService>();
            InstanceFactory.RegisterType<
                MsCampus.Win.Shared.Contracts.Services.INavigationService,
                Conference.Services.NavigationService>();
            InstanceFactory.RegisterType<IToastService, ToastService>();
            InstanceFactory.RegisterType<IStateService, StateService>();
            InstanceFactory.RegisterType<ICacheService, WindowsStorageCacheService>();
            InstanceFactory.RegisterType<IFlyoutService, FlyoutService>();
        }


        public IHomePageViewModel HomePageViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<IHomePageViewModel>();
            }
        }

        public ISessionDetailsFlyoutViewModel SessionDetailsFlyoutViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<ISessionDetailsFlyoutViewModel>();
            }
        }

        public ISpeakerDetailsFlyoutViewModel SpeakerDetailsFlyoutViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<ISpeakerDetailsFlyoutViewModel>();
            }
        }

        public IAboutAppSettingsFlyoutViewModel AboutAppSettingsFlyoutViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<IAboutAppSettingsFlyoutViewModel>();
            }
        }

        public IPrivacySettingsFlyoutViewModel PrivacySettingsFlyoutViewModel
        {
            get
            {
                return InstanceFactory.GetInstance<IPrivacySettingsFlyoutViewModel>();
            }
        }
    }
}
