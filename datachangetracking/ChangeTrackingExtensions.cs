using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GrumpyDev.Net.DataTools.ChangeTracking
{
    public static class ChangeTrackingExtensions
    {
        static IDictionary<object, IDictionary<string, ChangeTrackingInfo>> TrackedPropertyDictionary { get; set; }

        static ChangeTrackingExtensions()
        {
            TrackedPropertyDictionary = new Dictionary<object, IDictionary<string, ChangeTrackingInfo>>();
        }
        public static ChangeTrackingInfo GetChangeTrackingInfo<TEntity, TPropertyType>(this TEntity value, Expression<Func<TPropertyType>> expr)
        {
            //var type = value.GetType();

            //var fieldInfo = type.GetField(value.ToString());

            //var attributes = fieldInfo.GetCustomAttributes(typeof(TrackedFieldAttribute), false) as TrackedFieldAttribute[];

            //return attributes.Length > 0 ? attributes[0].ChangeTrackingInfo : null;

            //var props = typeof(TEntity).GetProperties();
            //foreach (PropertyInfo prop in props)
            //{
            //    object[] attrs = prop.GetCustomAttributes(true);
            //    foreach (object attr in attrs)
            //    {
            //        var authAttr = attr as TrackedFieldAttribute;
            //        if (authAttr != null)
            //        {
            //            return authAttr.ChangeTrackingInfo;
            //        }
            //    }
            //}

            var mexpr = expr.Body as MemberExpression;
            if (mexpr == null) return null;
            if (mexpr.Member == null) return null;


            return GetChangeTrackingInfo(value, mexpr.Member.Name);



            //object[] attrs = mexpr.Member.GetCustomAttributes(typeof(TrackedFieldAttribute), false);
            //if (attrs == null || attrs.Length == 0) return null;
            //TrackedFieldAttribute desc = attrs[0] as TrackedFieldAttribute;
            //if (desc == null) return null;
            //return desc.Description;
        }

        public static ChangeTrackingInfo GetChangeTrackingInfo<TEntity>(this TEntity value, string propertyName = "")
        {

            if (!TrackedPropertyDictionary.ContainsKey(value))
            {
                var newpropertyDictionary = new Dictionary<string, ChangeTrackingInfo>
                {
                    {propertyName, new ChangeTrackingInfo()}
                };

                TrackedPropertyDictionary.Add(value, newpropertyDictionary);
            }

            var propertyDictionary = TrackedPropertyDictionary[value];

            if (!propertyDictionary.ContainsKey(propertyName))
            {
                propertyDictionary.Add(propertyName, new ChangeTrackingInfo());
            }

            return propertyDictionary[propertyName];

        }
    }
}
