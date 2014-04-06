using MsCampus.Win.Shared.Contracts.Services;
using MsCampus.Win.Shared.DI;
using MsCampus.Win.Shared.Implementation.Services;
using Conference.App.Views;
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

namespace Conference.App.Common
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            //ViewModel registration
            InstanceFactory.RegisterType<IHomePageViewModel, HomePageViewModel>();
            InstanceFactory.RegisterWithTransientLifetime<ISessionDetailsFlyoutViewModel, SessionDetailsFlyoutViewModel>();
            InstanceFactory.RegisterWithTransientLifetime<ISpeakerDetailsFlyoutViewModel, SpeakerDetailsFlyoutViewModel>();
            InstanceFactory.RegisterType<IAboutAppSettingsFlyoutViewModel, AboutAppSettingsFlyoutViewModel>();
            InstanceFactory.RegisterType<IPrivacySettingsFlyoutViewModel, PrivacySettingsFlyoutViewModel>();

            //View registration
            InstanceFactory.RegisterType<IHomePageView, HomePageView>();
            InstanceFactory.RegisterWithTransientLifetime<ISessionDetailsFlyoutView, SessionDetailsFlyoutView>();
            InstanceFactory.RegisterWithTransientLifetime<ISpeakerDetailsFlyoutView, SpeakerDetailsFlyoutView>();
            InstanceFactory.RegisterType<IAboutAppSettingsFlyoutView, AboutAppSettingsFlyoutView>();
            InstanceFactory.RegisterType<IPrivacySettingsFlyoutView, PrivacySettingsFlyoutView>();

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
