using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace TaskDemo
{
    internal class ServiceInstaller<TService> : IWindsorInstaller where TService : class
    {
        private readonly Action<ComponentRegistration<TService>> _registration;

        public ServiceInstaller(Action<ComponentRegistration<TService>> registration = null)
        {
            this._registration = registration;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            ComponentRegistration<TService> componentRegistration = Component.For<TService>().ImplementedBy<TService>();
            if (_registration != null)
                _registration(componentRegistration);
            container.Register(new IRegistration[1]
            {
        (IRegistration) componentRegistration
            });
        }
    }
}