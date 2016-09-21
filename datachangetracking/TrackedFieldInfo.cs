using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrumpyDev.Net.DataTools.ChangeTracking
{
    public class TrackedFieldInfo
    {
        public object OldValue { get; set; }
        public FieldChangeState State { get; set; }
    }
}
