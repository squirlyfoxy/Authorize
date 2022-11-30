using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Core.H
{
    public enum PermitterClassManager
    {
        /// <summary>
        /// Permitter will work as an instance, registering and deregistering classType from Manager
        /// </summary>
        Instance,
        NoRegister
    }

    public class Permitter : IDisposable
    {
        public Permitter(Type classType, IPermitClass permit, PermitterClassManager classManager = PermitterClassManager.NoRegister)
        {
            PermitClass = permit;
            ClassType = classType;
            PermitterClassManager = classManager;

            if (!Manager.ClassRegistered(classType) && classManager == PermitterClassManager.Instance)
            {
                Manager.RegisterClass(classType);
            }
        }

        public Type ClassType { get; private set; }

        public IPermitClass PermitClass { get; private set; }

        public PermitterClassManager PermitterClassManager { get; private set; }

        public void Dispose()
        {
            if (PermitterClassManager == PermitterClassManager.Instance)
            {
                Manager.DeregisterClass(ClassType);
            }
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

        public static Permitter Instance<T>(T instance, PermitterClassManager classManager = PermitterClassManager.NoRegister) where T : IPermitClass
        {
            var type = typeof(T);

            return new Permitter(type, instance, classManager);
        }
    }
}
