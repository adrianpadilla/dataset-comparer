﻿using System;

namespace GrumpyDev.Net.DataTools.ChangeTracking
{
    public class TrackedFieldAttribute : Attribute
    {
        public TrackedFieldInfo TrackedFieldInfo { get; set; }

        public TrackedFieldAttribute()
        {
            this.TrackedFieldInfo = new TrackedFieldInfo();
        }
    }
}
