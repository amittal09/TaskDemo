using Castle.MicroKernel;
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
    internal class HostFactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IWindsorContainer iwindsorContainer = container;
            IRegistration[] iregistrationArray = new IRegistration[1];
            int index = 0;
            ComponentRegistration<IHostFactory> componentRegistration1 = Component.For<IHostFactory>();
            int num = 0;
            ComponentRegistration<IHostFactory> componentRegistration2 = componentRegistration1.UsingFactoryMethod(kernel => new HostFactory(kernel), num != 0);
            iregistrationArray[index] = (IRegistration)componentRegistration2;
            iwindsorContainer.Register(iregistrationArray);
        }

        private class HostFactory : IHostFactory
        {
            private readonly IKernel _kernel;

            public HostFactory(IKernel kernel)
            {
                this._kernel = kernel;
            }

            public IHost[] GetAll()
            {
                return (IHost[])this._kernel.ResolveAll<IHost>();
            }
        }
    }
}
