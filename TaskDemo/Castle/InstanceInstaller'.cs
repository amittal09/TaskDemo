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
    internal class InstanceInstaller<T> : IWindsorInstaller where T : class
    {
        private readonly T _instance;

        public InstanceInstaller(T instance)
        {
            if ((object)instance == null)
                throw new ArgumentNullException(nameof(instance));
            this._instance = instance;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(new IRegistration[1]
            {
                Component.For<T>().Instance(this._instance)
            });
        }
    }
}
