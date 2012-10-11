using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanblog.Model
{
    public class StreamItem : Entity
    {
        public string Name { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public int Ordinal { get { return this.GetValue<int>(); } set { this.SetValue(value); } }
        public bool IsDefault { get { return this.GetValue<bool>(); } set { this.SetValue(value); } }
        public bool IsActive { get { return this.GetValue<bool>(); } set { this.SetValue(value); } }

        public static async Task<IEnumerable<StreamItem>> GetActiveStreamsAsync()
        {
            var conn = KanblogRuntime.GetConnection();
            return await conn.Table<StreamItem>().Where(v => v.IsActive).OrderBy(v => v.Ordinal).ToListAsync();
        }

        internal static async Task CreateDefaultStreams()
        {
            var tickle = await CreateStreamAsync("Tickle");
            await tickle.AddStoryAsync("A ticklish idea", "'Tickle' ideas are ones that don't make sense now, but might in the future...");

            var ideas = await CreateStreamAsync("Ideas", true);
            await ideas.AddStoryAsync("An idea!", "This stream is where you collect ideas that occur to you, but before you have a chance to dig into them properly.");

            var planning = await CreateStreamAsync("Planning");
            await planning.AddStoryAsync("In planning", "Ideas in planning are ones that you are researching, structuring, etc.");

            var drafting = await CreateStreamAsync("Drafting");
            await drafting.AddStoryAsync("Writing!", "Ideas in draft are ones that you're actively working on.");

            var published = await CreateStreamAsync("Published");
            await published.AddStoryAsync("And finally...", "And over here are the ideas that you've transformed into published articles.");
        }

        private static async Task<StreamItem> CreateStreamAsync(string name, bool isDefault = false)
        {
            var stream = new StreamItem()
            {
                Name = name,
                Ordinal = await GetNextOrdinalAsync(),
                IsDefault = isDefault,
                IsActive = true
            };

            var conn = KanblogRuntime.GetConnection();
            await conn.InsertAsync(stream);

            return stream;
        }

        private static async Task<int> GetNextOrdinalAsync()
        {
            var conn = KanblogRuntime.GetConnection();
            if (await conn.Table<StreamItem>().FirstOrDefaultAsync() != null)
                return await conn.ExecuteScalarAsync<int>("select max(ordinal) from streamitem") + 1000;
            else
                return 1000;
        }

        internal async Task<IEnumerable<StoryItem>> GetActiveStoriesAsync()
        {
            var conn = KanblogRuntime.GetConnection();
            return await conn.Table<StoryItem>().Where(v => v.StreamId == this.Id && v.IsActive).ToListAsync();
        }

        internal async Task AddStoryAsync(string name, string theAbstract)
        {
            var story = new StoryItem()
            {
                Name = name,
                StreamId = this.Id,
                Abstract = theAbstract,
                IsActive = true
            };

            var conn = KanblogRuntime.GetConnection();
            await conn.InsertAsync(story);
        }

        public override string ToString()
        {
            return this.Name;
        }

        internal static async Task<StreamItem> GetDefaultStreamAsync()
        {
            var conn = KanblogRuntime.GetConnection();
            return await conn.Table<StreamItem>().Where(v => v.IsDefault).FirstAsync();
        }
    }
}
