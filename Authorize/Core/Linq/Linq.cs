using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Core.Linq
{
    public class Linq
    {
        public Linq()
        {
            WhereQueries = new List<LinqPermissionQueryRule>();
        }

        public List<LinqPermissionQueryRule> WhereQueries { get; set; }
    }
}
