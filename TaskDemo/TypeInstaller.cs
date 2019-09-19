using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace TaskDemo
{
    internal class TypeInstaller<TService> : IWindsorInstaller where TService : class
    {
        private readonly Type _type;
        private readonly Action<ComponentRegistration<TService>> _registration;

        public TypeInstaller(Type type, Action<ComponentRegistration<TService>> registration = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this._type = type;
            this._registration = registration;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            ComponentRegistration<TService> componentRegistration = Component.For<TService>().ImplementedBy(this._type);
            if (_registration != null)
                _registration(componentRegistration);
            container.Register(new IRegistration[1] { componentRegistration });
        }
    }
}