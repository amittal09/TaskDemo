using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TaskDemo
{
    public class CollectionInstaller<TService> : IWindsorInstaller
    {
        private readonly List<Assembly> _assemblies;
        private readonly List<Type> _ignoreTypes;

        internal CollectionInstaller()
        {
            this._assemblies = new List<Assembly>();
            this._ignoreTypes = new List<Type>();
        }

        public CollectionInstaller<TService> AddFromAssemblyOfThis<T>()
        {
            this._assemblies.Add(typeof(T).Assembly);
            return this;
        }

        public CollectionInstaller<TService> Ignore<T>()
        {
            this._ignoreTypes.Add(typeof(T));
            return this;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (Assembly assembly in this._assemblies.Distinct())
                container.Register(new IRegistration[1]
                {
            Classes.FromAssembly(assembly).BasedOn<TService>().WithServiceFromInterface(typeof (TService)).If( classType => !this._ignoreTypes.Any( ignoreType => ignoreType.IsAssignableFrom(classType)))
                });
            IWindsorContainer iwindsorContainer1 = container;
            IRegistration[] iregistrationArray1 = new IRegistration[1];
            int index1 = 0;
            var componentRegistration1 = Component.For<IEnumerable<TService>>();
            int num1 = 0;
            ComponentRegistration<IEnumerable<TService>> componentRegistration2 = componentRegistration1.UsingFactoryMethod(kernel => kernel.ResolveAll<TService>(), num1 != 0);
            iregistrationArray1[index1] = componentRegistration2;
            iwindsorContainer1.Register(iregistrationArray1);
            IWindsorContainer iwindsorContainer2 = container;
            IRegistration[] iregistrationArray2 = new IRegistration[1];
            int index2 = 0;
            var componentRegistration3 = Component.For<TService[]>();
            int num2 = 0;
            ComponentRegistration<TService[]> componentRegistration4 = componentRegistration3.UsingFactoryMethod(kernel => kernel.ResolveAll<TService>(), num2 != 0);
            iregistrationArray2[index2] = componentRegistration4;
            iwindsorContainer2.Register(iregistrationArray2);
        }
    }
}