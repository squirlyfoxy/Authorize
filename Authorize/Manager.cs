using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Authorize.Core;

namespace Authorize
{
    public class Manager
    {
        public static Type UserType { get; private set; }

        public static object LoggedUser { get; private set; }

        public static Type PermissionType { get; private set; }

        public static PropertyInfo PermissionProperty { get; private set; }

        private static PropertyInfo MaximumPermissionProperty { get; set; }

        private static PropertyInfo MinimumPermissionProperty { get; set; }

        private static PropertyInfo PermissionCodeProperty { get; set; }

        public static List<Core.Class> Classes { get; private set; } = new List<Core.Class>();

        public static void RegisterUserClass(Type userClass, PropertyInfo permissionProperty)
        {
            UserType = userClass;
            PermissionProperty = permissionProperty;

            RegisterClass(userClass);
        }

        public static void RegisterClass(Type classType)
        {
            if (classType.IsClass)
            {
                Classes.Add(new Class(classType));
            }
        }

        public static void RegisterPermissionClass(Type permissionClass, PropertyInfo minimumProperty, PropertyInfo maximumProperty, PropertyInfo permissionCodeProperrty)
        {
            if (!UserType.GetProperties().Any(x => x.PropertyType == permissionClass))
            {
                throw new Exception("Typeof permissonClass not found in user");
            }

            PermissionType = permissionClass;
            MaximumPermissionProperty = maximumProperty;
            MinimumPermissionProperty = minimumProperty;
            PermissionCodeProperty = permissionCodeProperrty;
        }

        public static void RegisterCurrentUser(object user)
        {
            if (user.GetType() != UserType)
            {
                throw new Exception("User type non match");
            }

            LoggedUser = user;
        }

        public static bool CanRead(int permission, string propertyGroup)
        {
            var permissions = (object[])LoggedUser.GetType().GetRuntimeProperties().FirstOrDefault(x => x.PropertyType == PermissionType).GetValue(LoggedUser);

            foreach (var perm in permissions)
            {
                var group = (string)perm.GetType().GetRuntimeProperty(PermissionCodeProperty.Name).GetValue(perm);
                var min = (int)perm.GetType().GetRuntimeProperty(MinimumPermissionProperty.Name).GetValue(perm);

                if (group == propertyGroup || propertyGroup == string.Empty)
                {
                    return min >= permission;
                }
            }

            return false;
        }

        public static bool CanWrite(int permission, string propertyGroup)
        {
            var permissions = (object[])LoggedUser.GetType().GetRuntimeProperties().FirstOrDefault(x => x.PropertyType == PermissionType).GetValue(LoggedUser);

            foreach (var perm in permissions)
            {
                var group = (string)perm.GetType().GetRuntimeProperty(PermissionCodeProperty.Name).GetValue(perm);
                var min = (int)perm.GetType().GetRuntimeProperty(MaximumPermissionProperty.Name).GetValue(perm);

                if (group == propertyGroup || propertyGroup == string.Empty)
                {
                    return min >= permission;
                }
            }

            return false;
        }

        public static object[] ExecuteLinq(int permission, string propertyGroup, object[] startingArr, Core.Linq.LinqPermissionQueryRule query)
        {
            if (CanRead(permission, propertyGroup))
            {
                return startingArr.Where(s => query.QueryFunction(s)).ToArray();
            }

            return null;
        }
    }
}
