using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    internal class HostsInstaller : IWindsorInstaller
    {
        private readonly Type[] _addHosts;
        private readonly Type[] _skipHosts;

        public HostsInstaller(Type[] addHosts, Type[] skipHosts)
        {
            this._addHosts = addHosts ?? new Type[0];
            this._skipHosts = skipHosts ?? new Type[0];
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (Type type in ((IEnumerable<Type>)this._addHosts).Except<Type>((IEnumerable<Type>)this._skipHosts).Distinct<Type>())
                container.Register(new IRegistration[1] { Component.For<IHost>().ImplementedBy(type) });
        }
    }
}
