using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanblog.Model
{
    public interface IViewModelHost
    {
        void ShowView<T>(object args)
            where T : ViewModel;

        void GoBack();

        Task ShowMessageAsync(ErrorBucket errors);

        Task ShowMessageAsync(string message);

        Task<bool> ConfirmAsync(string message = "Are you sure?");
    }
}
