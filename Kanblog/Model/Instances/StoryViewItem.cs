using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanblog.Model
{
    public class StoryViewItem : WrappedModelItem<StoryItem>
    {
        public string ImageUri { get { return GetValue<string>(); } set { SetValue(value); } }
        
        public StoryViewItem(StoryItem item)
            : base(item)
        {
            this.ImageUri = string.Format("ms-appx:///Assets/Stream.{0}.png", StreamViewItem.GetStreamColor(item.StreamId));
        }

        public string Name
        {
            get
            {
                return this.Item.Name;
            }
        }
    }
}
