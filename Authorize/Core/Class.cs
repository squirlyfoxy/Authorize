using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Core
{
    public class Class
    {
        public Class(Type classType)
        {
            ClassType = classType;
        }

        public Type ClassType { get; set; }

        public PropertyInfo[] GetProperties()
        {
            return ClassType.GetProperties().Where(p => p.GetCustomAttributes<Property>().Count() != 0).ToArray();
        }

        public static bool CanReadProperty<T>(PropertyInfo property)
        {
            foreach (var c in Manager.Classes)
            {
                if (c.ClassType == typeof(T))
                {
                    // Loop throught properties
                    foreach (var p in c.GetProperties())
                    {
                        if (p == property)
                        {
                            return p.GetCustomAttribute<Property>().CanRead();
                        }
                    }
                }
            }

            return false;
        }

        public static bool CanWriteProperty<T>(PropertyInfo property)
        {
            foreach (var c in Manager.Classes)
            {
                if (c.ClassType == typeof(T))
                {
                    // Loop throught properties
                    foreach (var p in c.GetProperties())
                    {
                        if (p == property)
                        {
                            return p.GetCustomAttribute<Property>().CanWrite();
                        }
                    }
                }
            }

            return false;
        }

        public static dynamic Get<T>(PropertyInfo property, object instance)
        {
            if (CanReadProperty<T>(property))
            {
                foreach (var c in Manager.Classes)
                {
                    if (c.ClassType == typeof(T))
                    {
                        // Loop throught properties
                        foreach (var p in c.GetProperties())
                        {
                            if (p == property)
                            {
                                return p.GetValue(instance);
                            }
                        }
                    }
                }
            }

            return null;
        }

        public static void Set<T>(PropertyInfo property, object instance, object value)
        {
            if (CanWriteProperty<T>(property))
            {
                foreach (var c in Manager.Classes)
                {
                    if (c.ClassType == typeof(T))
                    {
                        // Loop throught properties
                        foreach (var p in c.GetProperties())
                        {
                            if (p == property)
                            {
                                p.SetValue(instance, value);
                            }
                        }
                    }
                }
            }
        }

        public static object[] Query<T>(Authorize.Core.Linq.Linq linq, PropertyInfo property, object[] instance)
        {
            if (CanReadProperty<T>(property))
            {
                foreach (var c in Manager.Classes)
                {
                    if (c.ClassType == typeof(T))
                    {
                        // Loop throught properties
                        foreach (var p in c.GetProperties())
                        {
                            if (p == property)
                            {
                                return Manager.ExecuteLinq(p.GetCustomAttribute<Property>().MinimumGetPermission, p.GetCustomAttribute<Property>().PermissionCode, instance, linq.WhereQueries.Where(w => w.Permission == p.GetCustomAttribute<Property>().MinimumGetPermission).OrderBy(k => k.Permission).First());
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
