using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Core.Linq
{
    public class LinqProperty : Attribute
    {
        /// <summary>
        /// Specify a property that can be LinqEd
        /// </summary>
        public LinqProperty()
            : base()
        {

        }

        public static LinqProperty Base()
        {
            return new LinqProperty();
        }
    }
}
