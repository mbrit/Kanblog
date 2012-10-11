using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanblog.Model
{
    public class ViewModel<T> : ViewModel
        where T : ModelItem
    {
        public T Item { get { return this.GetValue<T>(); } private set { this.SetValue(value); } }

        public ViewModel(IViewModelHost host)
            : base(host)
        {
        }

        public override void Activated(object args)
        {
            base.Activated(args);

            // set...
            this.Item = (T)args;
        }
    }
}
