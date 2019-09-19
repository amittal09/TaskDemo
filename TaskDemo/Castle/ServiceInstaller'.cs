﻿using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace TaskDemo
{
    internal class ServiceInstaller<TService, TImplementation> : IWindsorInstaller where TService : class where TImplementation : class, TService
    {
        private readonly Action<ComponentRegistration<TService>> _registration;

        public ServiceInstaller(Action<ComponentRegistration<TService>> registration = null)
        {
            this._registration = registration;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            ComponentRegistration<TService> componentRegistration = Component.For<TService>().ImplementedBy<TImplementation>();
            if (this._registration != null)
                this._registration(componentRegistration);
            container.Register(new IRegistration[1]
            {
        (IRegistration) componentRegistration
            });
        }
    }
}