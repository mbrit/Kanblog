using Kanblog.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Kanblog
{
    public static class KanblogRuntime
    {
        public static async Task StartAsync()
        {
            // boot...
            var conn = GetConnection();
            await conn.CreateTablesAsync<StreamItem, StoryItem>();

            // do we have any streams?
            var streams = await StreamItem.GetActiveStreamsAsync();
            if (!(streams.Any()))
                await StreamItem.CreateDefaultStreams();
        }

        internal  static SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection("Kanblog.db");
        }

        public static async Task OpenUriAsync(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
