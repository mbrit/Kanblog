using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;

namespace Kanblog.Model
{
    public class StoryPageViewModel : ViewModel<StoryItem>
    {
        public ObservableCollection<StreamItem> Streams { get { return GetValue<ObservableCollection<StreamItem>>(); } set { SetValue(value); } }
        public StreamItem SelectedStream { get { return GetValue<StreamItem>(); } set { SetValue(value); } }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public StoryPageViewModel(IViewModelHost host)
            : base(host)
        {
            this.Streams = new ObservableCollection<StreamItem>();

            // commands...
            this.CancelCommand = new DelegateCommand((args) =>
            {
                //if(this.IsDirty && await this.Host.ConfirmAsync())
                //    this.Host.GoBack();
                //else
                    this.Host.GoBack();
            });

            this.SaveCommand = new DelegateCommand(async (args) => await HandleSaveAsync());

            this.DeleteCommand = new DelegateCommand(async (args) =>
            {
                if (await this.Host.ConfirmAsync())
                {
                    await this.Item.SoftDeleteAsync();
                    this.Host.GoBack();
                }
            });
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            //// set...
            //this.IsDirty = true;
        }

        public override async void Activated(object args)
        {
            base.Activated(args);

            this.Streams.Clear();
            foreach (var stream in await StreamItem.GetActiveStreamsAsync())
            {
                this.Streams.Add(stream);

                if (this.Item.StreamId == stream.Id)
                    this.SelectedStream = stream;
            }

            // reset...
            //this.IsDirty = false;
        }

        private async Task HandleSaveAsync()
        {
            // validate...
            var errors = new ErrorBucket();
            if (string.IsNullOrEmpty(this.Item.Name))
                errors.AddError("Name is required.");
            if (this.SelectedStream == null)
                errors.AddError("Stream is required.");

            // if...
            if (!(errors.HasErrors))
            {
                // set...
                this.Item.StreamId = this.SelectedStream.Id;

                // save...
                await this.Item.SaveChangesAsync();

                // back...
                this.Host.GoBack();
            }
            else
                await this.Host.ShowMessageAsync(errors);
        }

        public override void ShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if(this.Item.IsNew)
                return;

            // ok...
            var request = args.Request;
            request.Data.Properties.Title = string.Format("Story '{0}'", this.Item.Name);
            request.Data.Properties.Description = "Shared story from Kanblog";

            request.Data.SetText(string.Format("{0}\r\n\r\nAbstract:\r\n{1}\r\n\r\nNotes:\r\n{2}",
                this.Item.Name, this.Item.Abstract, this.Item.Notes));
        }
    }
}
