using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrumpyDev.Net.DataTools.ChangeTracking
{
    public class TrackedEntityInfo
    {
        public ChangeState State { get; set; }

        public TrackedEntityInfo()
        {
            this.State = ChangeState.Unmodified;
        }
    }
}
