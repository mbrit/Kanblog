using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanblog.Model
{
    public class StoryItem : Entity
    {
        public string Name { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string Abstract { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public string Notes { get { return this.GetValue<string>(); } set { this.SetValue(value); } }
        public int StreamId { get { return this.GetValue<int>(); } set { this.SetValue(value); } } 
        public bool IsActive { get { return this.GetValue<bool>(); } set { this.SetValue(value); } }

        internal static async Task<StoryItem> CreateForNewAsync()
        {
            var stream = await StreamItem.GetDefaultStreamAsync();

            var item = new StoryItem()
            {
                Name = "New Story",
                StreamId = stream.Id,
                IsActive = true
            };
            return item;
        }

        internal async Task SoftDeleteAsync()
        {
            this.IsActive = false;
            await this.SaveChangesAsync();
        }
    }
}
