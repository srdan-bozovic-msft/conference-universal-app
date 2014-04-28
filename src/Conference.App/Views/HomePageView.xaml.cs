using MsCampus.Win.Shared.Contracts.ViewModels;
using Conference.Contracts.Views;
using Conference.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Conference.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePageView : Page, IHomePageView
    {
        public HomePageView()
        {
            this.InitializeComponent();
            VisualStateManager.GoToState(this, "Sessions", false);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = this.DataContext as HomePageViewModel;
            vm.PropertyChanged += vm_PropertyChanged;
        }

        void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SessionGroupTileInfos"))
                this.sessionsGroupGridView.ItemsSource =
                    this.sessionGroupedItemsViewSource.View.CollectionGroups;
            if (e.PropertyName.Equals("SpeakerGroupTileInfos"))
                this.speakersGroupGridView.ItemsSource = 
                    this.speakerGroupedItemsViewSource.View.CollectionGroups;
            if (e.PropertyName.Equals("SelectedIndex"))
            {
                var viewModel = ViewModel as HomePageViewModel;
                switch (viewModel.SelectedIndex)
                {
                    case 0:
                        VisualStateManager.GoToState(this, "Sessions", false);
                        break;
                    case 1:
                        VisualStateManager.GoToState(this, "Speakers", false);
                        break;
                    default:
                        break;
                }
            }
        }

        public IPageViewModel ViewModel
        {
            get 
            { 
                return this.DataContext as IPageViewModel; 
            }
        }

        private double previousInfoButtonRightPoint = 0.0;
        private double previousTabItemsControlPortraitPoint = 0.0;
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var infoButtonTrans = infoButton.TransformToVisual(null);
            var infoButtonPoint = infoButtonTrans.TransformPoint(new Windows.Foundation.Point());
            var tabItemsControlPortraitTrans = tabItemsControlPortrait.TransformToVisual(null);
            var tabItemsControlPortraitPoint = tabItemsControlPortraitTrans.TransformPoint(new Windows.Foundation.Point());


            if (((e.NewSize.Height / e.NewSize.Width >= 1) || (infoButtonPoint.X + infoButton.ActualWidth > e.NewSize.Width)) && e.NewSize.Width > previousTabItemsControlPortraitPoint)
            {
                previousTabItemsControlPortraitPoint = tabItemsControlPortraitPoint.X + tabItemsControlPortrait.ActualWidth;
                previousInfoButtonRightPoint = infoButtonPoint.X + infoButton.ActualWidth;
                VisualStateManager.GoToState(this, "Portrait", true);
            }
            else if (tabItemsControlPortraitPoint.X + tabItemsControlPortrait.ActualWidth > e.NewSize.Width)
            {
                previousTabItemsControlPortraitPoint = tabItemsControlPortraitPoint.X + tabItemsControlPortrait.ActualWidth;
                previousInfoButtonRightPoint = infoButtonPoint.X + infoButton.ActualWidth;
                VisualStateManager.GoToState(this, "Narrow", true);
            }
            else if (e.NewSize.Width > previousInfoButtonRightPoint)
            {
                VisualStateManager.GoToState(this, "DefaultLayout", true);
            }
        }

    }
}
