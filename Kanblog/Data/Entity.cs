using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanblog.Model
{
    public abstract class Entity : ModelItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get { return this.GetValue<int>(); } set { this.SetValue(value); } }

        internal async Task SaveChangesAsync()
        {
            var conn = KanblogRuntime.GetConnection();
            if (this.IsNew)
                await conn.InsertAsync(this);
            else
                await conn.UpdateAsync(this);
        }

        public bool IsNew
        {
            get
            {
                return this.Id == 0;
            }
        }
    }
}
