using MsCampus.Win.Shared.Contracts.Services;
using MsCampus.Win.Shared.Contracts.Views;
using MsCampus.Win.Shared.CustomControls;
using MsCampus.Win.Shared.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsCampus.Win.Shared.Implementation.Services
{
    public class FlyoutService : IFlyoutService
    {
        public void Show<T>(object parameter)
            where T : IPageView
        {
            var view = InstanceFactory.GetInstance<T>();
            if (view != null)
            {
                view.ViewModel.Initialize(parameter);
                var flyout = view as CustomSettingsFlyout;      
                if (flyout != null)
                    flyout.Show();
            }
        }

        public void ShowIndependent<T>(object parameter)
            where T : IPageView
        {
            var view = InstanceFactory.GetInstance<T>();
            if (view != null)
            {
                view.ViewModel.Initialize(parameter);
                var flyout = view as CustomSettingsFlyout;
                if (flyout != null)
                    flyout.ShowIndependent();
            }
        }
    }
}
