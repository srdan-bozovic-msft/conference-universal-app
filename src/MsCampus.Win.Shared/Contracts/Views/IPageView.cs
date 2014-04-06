using MsCampus.Win.Shared.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsCampus.Win.Shared.Contracts.Views
{
    public interface IPageView
    {
        IPageViewModel ViewModel { get; }
    }
}
