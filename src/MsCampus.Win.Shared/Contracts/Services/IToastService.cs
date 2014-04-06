using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsCampus.Win.Shared.Contracts.Services
{
    public interface IToastService
    {
        void SendSimpleTextToast(string text);
        void SendImageAndTextToast(string text, string imageUrl);
    }
}
