using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Kanblog.Model
{
    public abstract class ViewModel : ModelItem
    {
        //public bool IsDirty { get { return this.GetValue<bool>(); } set { this.SetValue(value); } }

        protected IViewModelHost Host { get; private set; }

        protected ViewModel(IViewModelHost host)
        {
            this.Host = host;
        }

        public virtual void Activated(object args)
        {
        }

        public virtual void ShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
        }
    }
}
