using MsCampus.Win.Shared.Contracts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MsCampus.Win.Shared.Contracts.Services
{
    public interface IFlyoutService
    {
        void Show<T>(object parameter)
            where T : IPageView;

        void ShowIndependent<T>(object parameter)
            where T : IPageView;
    }
}
