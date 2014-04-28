using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace MsCampus.Win.Shared.CustomControls
{
    public class CustomSettingsFlyout : SettingsFlyout
    {
        private bool _back;
        private Popup _popup;

        public static readonly DependencyProperty IsLightDismissedEnabledProperty =
            DependencyProperty.Register(
            "IsLightDismissedEnabled",
            typeof(bool),
            typeof(CustomSettingsFlyout),
            new PropertyMetadata(true, new PropertyChangedCallback(isLightDismissedEnabledPropertyChangedCallback)));

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(
            "IsOpen",
            typeof(bool),
            typeof(CustomSettingsFlyout),
            new PropertyMetadata(true, new PropertyChangedCallback(isOpenPropertyChangedCallback)));

        private static void isOpenPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                (d as CustomSettingsFlyout)._popup.IsOpen = (bool)e.NewValue;
                (d as CustomSettingsFlyout).IsOpen = (bool)e.NewValue;
            }
        }

        private static void isLightDismissedEnabledPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                (d as CustomSettingsFlyout)._popup.IsLightDismissEnabled = (bool)e.NewValue;
                (d as CustomSettingsFlyout).IsLightDismissedEnabled = (bool)e.NewValue;
                if ((bool)e.NewValue == true)
                {
                    (d as CustomSettingsFlyout).Hide();
                    (d as CustomSettingsFlyout).ShowIndependent();
                }
            }
        }
        public bool IsLightDismissedEnabled
        {
            get { return (bool)GetValue(IsLightDismissedEnabledProperty); }
            set { SetValue(IsLightDismissedEnabledProperty, value); }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public new void ShowIndependent()
        {
            base.ShowIndependent();
            _popup = (Parent as Popup);
            _popup.Closed += Popup_Closed;
            BackClick += CustomSettingsFlyout_BackClick;
        }

        public new void Show()
        {
            base.Show();
            _popup = (Parent as Popup);
            _popup.Closed += Popup_Closed;
            BackClick += CustomSettingsFlyout_BackClick;
        }

        private void CustomSettingsFlyout_BackClick(object sender, BackClickEventArgs e)
        {
            _back = true;
        }

        private void Popup_Closed(object sender, object e)
        {
            if (!_back && IsLightDismissedEnabled == false) _popup.IsOpen = true;
        }
    }
}
