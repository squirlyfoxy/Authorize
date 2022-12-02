using System;
using System.Reflection;

using Authorize.Core;
using Authorize.Core.H;
using Authorize.Core.Linq;

namespace Test
{
    public class User : IPermitClass
    {
        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced, "Nna")]
        public string Name { get; set; }

        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced)]
        public string Surname { get; set; }

        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced, "Nna")]
        [LinqProperty]
        public string[] Friends { get; set; }

        public Permission[] Permissions { get; set; }

        public bool CanAccessCustom(PropertyInfo target)
        {
            throw new NotImplementedException();
        }

        public bool CanWriteCustom(PropertyInfo target)
        {
            throw new NotImplementedException();
        }
    }
}
