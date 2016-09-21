using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrumpyDev.Net.DataTools.ChangeTracking
{
    public class TrackedEntityAttribute : Attribute
    {
        public TrackedEntityInfo TrackedEntityInfo { get; set; }

        public TrackedEntityAttribute()
        {
            this.TrackedEntityInfo =  new TrackedEntityInfo();
        }
    }
}
