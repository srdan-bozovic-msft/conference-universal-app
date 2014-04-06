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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Conference.App.Views
{
    public sealed partial class SpeakerDetailsFlyoutView : SettingsFlyout, ISpeakerDetailsFlyoutView
    {
        public SpeakerDetailsFlyoutView()
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
                WebView.NavigateToString("<html><head><script type='text/javascript'>" + f + "</script><style>*{font-family:'Segoe UI';}</style></head><body>" + bio + "</body></html>");
        }

        public IPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as IPageViewModel;
            }
        }

    }
}
