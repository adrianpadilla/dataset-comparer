﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrumpyDev.Net.DataTools.ChangeTracking
{
    public enum ChangeState
    {
        Unmodified,

        New,

        Modified,

        Deleted
    }
}
