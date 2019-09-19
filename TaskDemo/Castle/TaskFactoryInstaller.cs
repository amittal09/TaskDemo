using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskDemo
{
    internal class TaskFactoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)

        {
            IWindsorContainer iwindsorContainer = container;
            IRegistration[] iregistrationArray = new IRegistration[1];
            int index = 0;
            ComponentRegistration<ITaskFactory> componentRegistration1 = Component.For<ITaskFactory>();
            int num = 0;
            ComponentRegistration<ITaskFactory> componentRegistration2 = componentRegistration1.UsingFactoryMethod(kernel => new TaskFactory(kernel), num != 0);
            iregistrationArray[index] = componentRegistration2;
            iwindsorContainer.Register(iregistrationArray);
        }

        private class TaskFactory : ITaskFactory
        {
            private readonly IKernel _kernel;

            public TaskFactory(IKernel kernel)
            {
                this._kernel = kernel;
            }

            public bool Exists(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Value cannot be null or empty.", nameof(name));
                IHandler handler = this._kernel.GetHandler(name);
                if (handler != null)
                    return typeof(ITask).IsAssignableFrom(handler.ComponentModel.Implementation);
                return false;
            }

            public ITask Get<TTask>() where TTask : class, ITask
            {
                return this.Get(Task.NameOf<TTask>());
            }

            public ITask Get(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Value cannot be null or empty.", name);
                if (!this.Exists(name))
                    throw new Exception(name);
                return (ITask)this._kernel.Resolve<ITask>(name);
            }

            public bool TryGet(string name, out ITask task)
            {
                task = (ITask)null;
                if (this.Exists(name))
                    task = this.Get(name);
                return task != null;
            }

            public ITask[] GetAll()
            {
                return ((IEnumerable<ITask>)_kernel.ResolveAll<ITask>()).OrderBy(x => x.Name()).ToArray();
            }
        }
    }
}
