using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public enum PermissionRead
    {
        ReadBasic = 10,
        ReadAdvanced = 100
    }

    public enum PermissionWrite
    {
        WriteBasic = 10,
        WriteAdvanced = 100
    }

    public class Permission
    {
        public string PermissionCode { get; set; }

        public PermissionRead Read { get; set; }

        public PermissionWrite Write { get; set; }
    }
}
