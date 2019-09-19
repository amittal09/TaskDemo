using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TaskDemo
{
    public class ConventionInstaller : IWindsorInstaller
    {
        private readonly List<Assembly> _assemblies;
        private readonly List<Type> _ignoreTypes;

        internal ConventionInstaller()
        {
            this._assemblies = new List<Assembly>();
            this._ignoreTypes = new List<Type>();
        }

        public ConventionInstaller AddFromAssemblyOfThis<T>()
        {
            this._assemblies.Add(typeof(T).Assembly);
            return this;
        }

        public ConventionInstaller Ignore<T>()
        {
            this._ignoreTypes.Add(typeof(T));
            return this;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (Assembly assembly in this._assemblies.Distinct<Assembly>())
                container.Register(new IRegistration[1]
                {
          Classes.FromAssembly(assembly).Pick().If( classType =>
          {
            if (((IEnumerable<Type>) classType.GetInterfaces()).Any( classInterface => _assemblies.Contains(classInterface.Assembly)))
              return !this._ignoreTypes.Any( ignoreType => ignoreType.IsAssignableFrom(classType));
            return false;
          }).WithService.DefaultInterfaces().Configure( registration => SetLifestyle( registration.IsFallback()))
                });
        }

        private static ComponentRegistration<object> SetLifestyle(ComponentRegistration<object> registration)
        {
            if (!Attribute.IsDefined((MemberInfo)registration.Implementation, typeof(LifestyleAttribute)))
                return registration.LifeStyle.Singleton;
            return registration;
        }
    }
}