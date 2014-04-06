using MsCampus.Win.Shared.Contracts.Services;
using MsCampus.Win.Shared.Contracts.Views;
using MsCampus.Win.Shared.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MsCampus.Win.Shared.Implementation.Services
{
    public abstract class NavigationServiceBase : INavigationService
    {
        private Frame _frame;
        public Frame Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
                _frame.Navigated += OnFrameNavigated;
            }
        }

        public NavigationServiceBase()
        {

        }

        public void Navigate(Type type)
        {
            Frame.Navigate(type);
        }

        public abstract void Navigate(string type, object parameter);

        public void Navigate(Type type, object parameter)
        {
            Frame.Navigate(type, parameter);
        }

        protected void Navigate<T>(object parameter) where T : IPageView
        {
            DisposePreviousView();
            var viewType = InstanceFactory.Registrations.ContainsKey(typeof(T)) ? InstanceFactory.Registrations[typeof(T)] : typeof(T);
            Frame.Navigate(viewType, parameter);
        }

        public IPageView CurrentView
        {
            get { return Frame.Content as IPageView; }
        }

        private void DisposePreviousView()
        {
            try
            {
                if (this.CurrentView != null && ((Page)this.CurrentView).NavigationCacheMode ==
                    Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled)
                {
                    var currentView = this.CurrentView;
                    var currentViewDisposable = currentView as IDisposable;

                    // if(currentView is BasePage
                    if (currentViewDisposable != null)
                    {
                        currentViewDisposable.Dispose();
                        currentViewDisposable = null;
                    } //view model is disposed in the view implementation
                    currentView = null;
                    GC.Collect();
                }
            }
            catch { }
        }

        public void Navigate(string type)
        {
            Frame.Navigate(Type.GetType(type));
        }

        public void GoBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void OnFrameNavigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var view = e.Content as IPageView;
            if (view == null)
                return;

            var viewModel = view.ViewModel;
            if (viewModel != null)
            {
                if (!(e.NavigationMode ==
                    Windows.UI.Xaml.Navigation.NavigationMode.Back
                    &&
                    (((Page)e.Content).NavigationCacheMode ==
                    Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled ||
                    (((Page)e.Content).NavigationCacheMode ==
                    Windows.UI.Xaml.Navigation.NavigationCacheMode.Required))))
                {
                    viewModel.Initialize(e.Parameter);
                }
            }
        }
    }
}
