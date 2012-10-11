using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanblog.Model
{
    public class WrappedModelItem<T> : ModelItem
        where T : ModelItem
    {
        public T Item { get; private set; }

        public WrappedModelItem(T item)
        {
            this.Item = item;
        }
    }
}
