using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GrumpyDev.Net.DataTools.ChangeTracking
{
    public static class ChangeTrackingExtensions
    {
        public static TrackedFieldInfo GetTrackedFieldInfo<TEntity>(this TEntity value)
        {
            var type = value.GetType();

            var fieldInfo = type.GetField(value.ToString());

            var attributes = fieldInfo.GetCustomAttributes(typeof(TrackedFieldAttribute), false) as TrackedFieldAttribute[];

            return attributes.Length > 0 ? attributes[0].TrackedFieldInfo : null;
        }

        public static TrackedEntityInfo GetTrackedEntityInfo<TEntity>(this TEntity value)
        {
            var type = value.GetType();

            var fieldInfo = type.GetField(value.ToString());

            var attributes = fieldInfo.GetCustomAttributes(typeof(TrackedEntityAttribute), false) as TrackedEntityAttribute[];

            return attributes.Length > 0 ? attributes[0].TrackedEntityInfo : null;
        }
    }
}
