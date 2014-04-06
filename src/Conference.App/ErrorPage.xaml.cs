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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Conference.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ErrorPage : Page
    {
        public ErrorPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Frame.BackStackDepth == 1)
                backButton.Visibility = Visibility.Collapsed;
            ErrorMessage.Text = "Došlo je do problema prilikom preuzimanja podataka.\nMolimo Vas pokušajte kasnije.";
        }

        private void Image_Tap(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
            Frame.GoBack();
        }
    }
}
