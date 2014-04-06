using Conference.Contracts.Views;
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
    public sealed partial class PrivacySettingsFlyoutView : SettingsFlyout, IPrivacySettingsFlyoutView
    {
        public PrivacySettingsFlyoutView()
        {
            this.InitializeComponent();
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
