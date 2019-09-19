using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;

namespace TaskDemo
{
    public class HostsConfiguration : IInitializable<IWindsorContainer>
    {
        private readonly List<Type> _add;
        private readonly List<Type> _remove;

        internal HostsConfiguration(ApplicationConfiguration application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));
            this.Application = application;
            this._add = new List<Type>();
            this._remove = new List<Type>();
            this.Host<TaskHost>();
        }

        public ApplicationConfiguration Application { get; private set; }

        public HostsConfiguration Host<THost>() where THost : IHost
        {
            this._add.Add(typeof(THost));
            return this;
        }

        public HostsConfiguration Remove<THost>() where THost : IHost
        {
            this._remove.Add(typeof(THost));
            return this;
        }

        void IInitializable<IWindsorContainer>.Initialize(IWindsorContainer container)
        {
            container.Install(new IWindsorInstaller[1]
            {
        (IWindsorInstaller) new HostsInstaller(this._add.ToArray(), this._remove.ToArray())
            });
            container.Install(new IWindsorInstaller[1]
            {
        (IWindsorInstaller) new HostFactoryInstaller()
            });
        }

        public HostsConfiguration Clear()
        {
            this._add.Clear();
            this._remove.Clear();
            return this;
        }
    }
}
