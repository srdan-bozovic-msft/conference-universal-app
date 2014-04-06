using Conference.Contracts.ViewModels;
using Conference.Contracts.Views;
using GalaSoft.MvvmLight;
using MsCampus.Win.Shared.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Conference.PhoneApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SpeakerDetailsPageView : Page, ISpeakerDetailsFlyoutView
    {
        public SpeakerDetailsPageView()
        {
            this.InitializeComponent();
            var vm = DataContext as ViewModelBase;
            vm.PropertyChanged += vm_PropertyChanged;
            WebView.NavigationCompleted += async (s, e) =>
            {
                var height = await WebView.InvokeScriptAsync("getDocHeight", null);
                if (!string.IsNullOrEmpty(height))
                    WebView.Height = int.Parse(height);
            };
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            
            e.Handled = true;
            if(Frame.CanGoBack)
            Frame.GoBack();
           // 
        }


        void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Bio")
                return;
            var f = "function getDocHeight() {" +
                "var D = document;" +
                "return Math.max(" +
                    "Math.max(D.body.scrollHeight, D.documentElement.scrollHeight)," +
                    "Math.max(D.body.offsetHeight, D.documentElement.offsetHeight)," +
                    "Math.max(D.body.clientHeight, D.documentElement.clientHeight) " +
                ").toString();" +
            "}";
            var bio = (ViewModel as ISpeakerDetailsFlyoutViewModel).Bio;
            if (!string.IsNullOrEmpty(bio))
                WebView.NavigateToString("<html><head><script type='text/javascript'>" + f + "</script><style>*{font-family:'Georgia';color:#333332} #content{margin:20px;font-size:40px;line-height:60px;}</style></head><body><div id='content'>" + bio + "</div></body></html>");
        }

        public IPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as IPageViewModel;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }
    }
}
