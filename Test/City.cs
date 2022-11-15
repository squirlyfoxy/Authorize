using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
