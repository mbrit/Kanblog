using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanblog.Model
{
    public class StreamViewItem : WrappedModelItem<StreamItem>
    {
        public ObservableCollection<StoryViewItem> Stories { get { return GetValue<ObservableCollection<StoryViewItem>>(); } set { SetValue(value); } }

        public StreamViewItem(StreamItem item)
            : base(item)
        {
            this.Stories = new ObservableCollection<StoryViewItem>();
        }

        public string Name
        {
            get
            {
                return this.Item.Name;
            }
        }

        internal async Task InitializeAsync()
        {
            this.Stories.Clear();
            foreach (var story in await this.Item.GetActiveStoriesAsync())
                this.Stories.Add(new StoryViewItem(story));
        }

        internal static StreamColour GetStreamColor(int id)
        {
            return (StreamColour)(id % 6);
        }
    }
}
