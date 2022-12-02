using System;
using System.Diagnostics;
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
                    // Check if custom Read method is implemented
                    try
                    {
                        var typeInstance = Activator.CreateInstance(typeof(T));

                        if (typeInstance is H.IPermitClass)
                        {
                            var permitClass = (H.IPermitClass)typeInstance;
                            return permitClass.CanAccessCustom(property);
                        }
                    }
                    catch (NotImplementedException)
                    {
                        // CanAccessCustom not implemented, let's try with properties
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("WARNING [Authorize]: " + ex.Message);
                    }

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

        public static bool CanReadProperty(Type type, PropertyInfo property)
        {
            foreach (var c in Manager.Classes)
            {
                if (c.ClassType == type)
                {
                    // Check if custom Read method is implemented
                    try
                    {
                        var typeInstance = Activator.CreateInstance(type);

                        if (typeInstance is H.IPermitClass)
                        {
                            var permitClass = (H.IPermitClass)typeInstance;
                            return permitClass.CanAccessCustom(property);
                        }
                    }
                    catch (NotImplementedException)
                    {
                        // CanAccessCustom not implemented, let's try with properties
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("WARNING [Authorize]: " + ex.Message);
                    }

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
                    try
                    {
                        var typeInstance = Activator.CreateInstance(typeof(T));

                        if (typeInstance is H.IPermitClass)
                        {
                            var permitClass = (H.IPermitClass)typeInstance;
                            return permitClass.CanWriteCustom(property);
                        }
                    }
                    catch (NotImplementedException)
                    {
                        // CanWriteProperty not implemented, let's try with properties
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("WARNING [Authorize]: " + ex.Message);
                    }

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

        public static bool CanWriteProperty(Type classType, PropertyInfo property)
        {
            foreach (var c in Manager.Classes)
            {
                if (c.ClassType == classType)
                {
                    try
                    {
                        var typeInstance = Activator.CreateInstance(classType);

                        if (typeInstance is H.IPermitClass)
                        {
                            var permitClass = (H.IPermitClass)typeInstance;
                            return permitClass.CanWriteCustom(property);
                        }
                    }
                    catch (NotImplementedException)
                    {
                        // CanWriteProperty not implemented, let's try with properties
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("WARNING [Authorize]: " + ex.Message);
                    }

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

        public static dynamic Get(Type type, PropertyInfo property, object instance)
        {
            if (CanReadProperty(type, property))
            {
                foreach (var c in Manager.Classes)
                {
                    if (c.ClassType == type)
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

        public static void Set(Type classType, PropertyInfo property, object instance, object value)
        {
            if (CanWriteProperty(classType, property))
            {
                foreach (var c in Manager.Classes)
                {
                    if (c.ClassType == classType)
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
                                if (p.GetCustomAttribute<Linq.LinqProperty>().GetType() == Linq.LinqProperty.Base().GetType())
                                {
                                    return Manager.ExecuteLinq(p.GetCustomAttribute<Property>().MinimumGetPermission, p.GetCustomAttribute<Property>().PermissionCode, instance, linq.WhereQueries.Where(w => w.Permission == p.GetCustomAttribute<Property>().MinimumGetPermission).OrderBy(k => k.Permission).First());
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        public static object[] Query(Type classType, Authorize.Core.Linq.Linq linq, PropertyInfo property, object[] instance)
        {
            if (CanReadProperty(classType, property))
            {
                foreach (var c in Manager.Classes)
                {
                    if (c.ClassType == classType)
                    {
                        // Loop throught properties
                        foreach (var p in c.GetProperties())
                        {
                            if (p == property)
                            {
                                if (p.GetCustomAttribute<Linq.LinqProperty>().GetType() == Linq.LinqProperty.Base().GetType())
                                {
                                    return Manager.ExecuteLinq(p.GetCustomAttribute<Property>().MinimumGetPermission, p.GetCustomAttribute<Property>().PermissionCode, instance, linq.WhereQueries.Where(w => w.Permission == p.GetCustomAttribute<Property>().MinimumGetPermission).OrderBy(k => k.Permission).First());
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
