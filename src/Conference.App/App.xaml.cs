using MsCampus.Win.Shared.Contracts.Services;
using MsCampus.Win.Shared.DI;
using Conference.App.Common;
using Conference.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ApplicationSettings;
using MsCampus.Win.Shared.Implementation.Services;
using Conference.Contracts.Views;
using Windows.UI.Notifications;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace Conference.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += Application_UnhandledException;    
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;
            var navigationService = InstanceFactory.GetInstance<INavigationService>();
            var stateService = InstanceFactory.GetInstance<IStateService>();

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                navigationService.Frame = rootFrame;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    stateService.LoadState();
                    if (stateService.Parameter != null && stateService.ViewName != null)
                    {
                        navigationService.Navigate(stateService.ViewName, stateService.Parameter);
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                navigationService.Navigate(NavigationService.PageNames.SpeakersPageView, null);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            var stateService = InstanceFactory.GetInstance<IStateService>();
            stateService.SaveState();
            deferral.Complete();
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            PeriodicUpdateRecurrence recurrence = PeriodicUpdateRecurrence.HalfHour;
            TileUpdateManager.CreateTileUpdaterForApplication().StartPeriodicUpdateBatch(

                new Uri[]
                {
                    new Uri("http://tarabica.azure-mobile.net/api/gettilexml?id=1"),
                    new Uri("http://tarabica.azure-mobile.net/api/gettilexml?id=2"),
                    new Uri("http://tarabica.azure-mobile.net/api/gettilexml?id=3"),
                    new Uri("http://tarabica.azure-mobile.net/api/gettilexml?id=4"),
                    new Uri("http://tarabica.azure-mobile.net/api/gettilexml?id=5")
                }, recurrence);
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {

            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "O aplikaciji", "O aplikaciji", (handler) => (new FlyoutService()).Show<IAboutAppSettingsFlyoutView>(null)));

            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "Politika privatnosti", "Politika privatnosti", (handler) => (new FlyoutService()).Show<IPrivacySettingsFlyoutView>(null)));
        }

        private void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(ErrorPage));
        }
    }
}
