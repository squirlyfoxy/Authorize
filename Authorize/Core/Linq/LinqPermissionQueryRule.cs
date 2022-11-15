using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Core.Linq
{
    public class LinqPermissionQueryRule
    {
        public LinqPermissionQueryRule(int permission, Func<dynamic, bool> queryFunction)
        {
            Permission = permission;
            QueryFunction = queryFunction;
        }

        public int Permission { get; set; }

        public Func<dynamic, bool> QueryFunction { get; set; }
    }
}
