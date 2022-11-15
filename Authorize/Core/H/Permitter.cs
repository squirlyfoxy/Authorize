using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Core.H
{
    public class Permitter : IDisposable
    {
        public Permitter(Type classType, IPermitClass permit)
        {
            PermitClass = permit;
            ClassType = classType;
        }

        public Type ClassType { get; private set; }

        public IPermitClass PermitClass { get; private set; }

        public void Dispose()
        {

        }

        public dynamic Get(string propertyName)
        {
            var property = ClassType.GetProperties().FirstOrDefault(p => p.Name == propertyName);

            return Class.Get(ClassType, property, PermitClass);
        }

        public void Set(string propertyName, object value)
        {
            var property = ClassType.GetProperties().FirstOrDefault(p => p.Name == propertyName);

            Class.Set(ClassType, property, PermitClass, value);
        }

        public object[] Where(string propertyName, Linq.Linq linq, object[] instance)
        {
            var property = ClassType.GetProperties().FirstOrDefault(p => p.Name == propertyName);

            return Class.Query(ClassType, linq, property, instance);
        }

        public static Permitter Instance<T>(T instance) where T : IPermitClass
        {
            var type = typeof(T);

            return new Permitter(type, instance);
        }
    }
}
