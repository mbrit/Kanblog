using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kanblog.Model
{
    public class StreamsPageViewModel : ViewModel
    {
        public ObservableCollection<StreamViewItem> Streams { get { return GetValue<ObservableCollection<StreamViewItem>>(); } set { SetValue(value); } }

        public ICommand NewCommand { get; private set; }

        public StreamsPageViewModel(IViewModelHost host)
            : base(host)
        {
            this.Streams = new ObservableCollection<StreamViewItem>();

            this.NewCommand = new DelegateCommand(async (args) =>
            {
                this.Host.ShowView<StoryPageViewModel>(await StoryItem.CreateForNewAsync());
            });
        }

        public override async void Activated(object args)
        {
            this.Streams.Clear();
            foreach (var stream in await StreamItem.GetActiveStreamsAsync())
            {
                var item = new StreamViewItem(stream);
                this.Streams.Add(item);

                // wait...
                await item.InitializeAsync();
            }
        }
    }
}
