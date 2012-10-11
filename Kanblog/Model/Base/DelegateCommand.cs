using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kanblog.Model
{
    public class DelegateCommand : ICommand
    {
        private Action<object> Callback { get; set; }

        public DelegateCommand(Action<object> callback)
        {
            this.Callback = callback;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.Callback(parameter);
        }
    }
}
