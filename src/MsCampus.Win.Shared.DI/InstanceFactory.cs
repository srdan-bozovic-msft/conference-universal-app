using MetroIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsCampus.Win.Shared.DI
{
    public static class InstanceFactory
    {
        static readonly IContainer _container;

        public static Dictionary<Type, Type> Registrations =
            new Dictionary<Type, Type>();

        static InstanceFactory()
        {
            _container = new MetroContainer();
        }

        public static void RegisterType<T1, T2>() where T2 : class,T1
        {
            Registrations[typeof(T1)] = typeof(T2);
            _container.Register<T1, T2>(null, new Singleton());
        }

        public static void RegisterInstance<T1>(T1 instance)
        {
            _container.RegisterInstance<T1>(instance);
        }

        public static void RegisterWithTransientLifetime<T1, T2>() where T2 : class,T1
        {
            Registrations[typeof(T1)] = typeof(T2);
            _container.Register<T1, T2>(registration: new Transient());
        }

        public static void RegisterNamedInstance<T1, T2>(string key) where T2 : class,T1
        {
            _container.Register<T1, T2>(key: key);
        }

        public static T GetInstance<T>()
        {
            return _container.Resolve<T>();
        }

        public static T GetNamedInstance<T>(string key)
        {
            return _container.Resolve<T>(key: key);
        }
    }
}
