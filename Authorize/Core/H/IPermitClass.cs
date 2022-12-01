using System.Reflection;

namespace Authorize.Core.H
{
    public interface IPermitClass
    {
        /// <summary>
        /// Custom method that allows to specify a custom rule to access the class.
        /// </summary>
        /// <returns>True: can access, False: can't access. If NotImplementedException the method is not considered.</returns>
        bool CanAccessCustom(PropertyInfo target);
    }
}
