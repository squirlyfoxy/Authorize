using System;
using System.Reflection;

using Authorize.Core;
using Authorize.Core.H;
using Authorize.Core.Linq;

namespace Test
{
    public class City : IPermitClass
    {
        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced, "City")]
        public string Name { get; set; }

        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced, "City")]
        public string Country { get; set; }

        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced, "City")]
        [LinqProperty]
        public User[] UsersThatLiveHere { get; set; }

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
