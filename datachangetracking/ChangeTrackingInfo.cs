using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrumpyDev.Net.DataTools.ChangeTracking
{
    public class ChangeTrackingInfo
    {
        public object OldValue { get; set; }
        public ChangeState State { get; set; }

        public IDictionary<string, object> CustomProperties { get; private set; }

        public ChangeTrackingInfo()
        {
            this.State = ChangeState.Unmodified;
            this.CustomProperties = new Dictionary<string, object>();
        }

        public T GetCustomProperty<T>(string key)
        {
            if (!this.CustomProperties.ContainsKey(key))
            {
                return default(T);
            }
            return (T)this.CustomProperties[key];
        }

        public void SetCustomProperty<T>(string key, T value)
        {
            this.CustomProperties.Add(key, value);
        }
    }
}
