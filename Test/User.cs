using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Authorize.Core;

namespace Test
{
    public class User
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
