using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Property : Attribute
    {
        public Property(int minGet, int minSet, string permissionCode = "")
            : base()
        {
            MinimumGetPermission = minGet;
            MinimumSetPermission = minSet;
            PermissionCode = permissionCode;
        }

        public int MinimumGetPermission { get; set; }

        public int MinimumSetPermission { get; set; }

        public string PermissionCode { get; set; }

        public bool CanRead()
        {
            return Manager.CanRead(MinimumGetPermission, PermissionCode);
        }

        public bool CanWrite()
        {
            return Manager.CanWrite(MinimumSetPermission, PermissionCode);
        }
    }
}
