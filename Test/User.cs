using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Authorize.Core;
using Authorize.Core.H;

namespace Test
{
    public class User : IPermitClass
    {
        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced, "Nna")]
        public string Name { get; set; }

        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced)]
        public string Surname { get; set; }

        [Property((int)PermissionRead.ReadBasic, (int)PermissionWrite.WriteAdvanced, "Nna")]
        public string[] Friends { get; set; }

        public Permission[] Permissions { get; set; }
    }
}
