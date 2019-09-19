using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TaskDemo
{
    public class TaskInstaller : IWindsorInstaller
    {
        private readonly Assembly[] _scanAssemblies;
        private readonly Type[] _addTasks;
        private readonly Type[] _ignoreTasks;

        public TaskInstaller(Assembly[] scanAssemblies, Type[] addTasks, Type[] ignoreTasks)
        {
            _scanAssemblies = scanAssemblies ?? new Assembly[0];
            _addTasks = addTasks ?? new Type[0];
            _ignoreTasks = ignoreTasks ?? new Type[0];
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (Assembly assembly in ((IEnumerable<Assembly>)this._scanAssemblies).Distinct())
                _ = container.Register(new IRegistration[1]
                {
            Classes.FromAssembly(assembly).BasedOn<Task>().Unless(new Predicate<Type>((_ignoreTasks).Contains)).Unless(new Predicate<Type>((_addTasks).Contains)).Configure( x =>
          {
            string str =  x.Implementation.TaskName();
            if (container.Kernel.HasComponent(str))
              throw new TaskWithSameNameAlreadyRegistredException( x.Implementation);
             x.Named(str);
          }).WithServiceDefaultInterfaces()
                });
            foreach (Type task in ((IEnumerable<Type>)_addTasks).Except(_ignoreTasks).Distinct())
            {
                try
                {
                    container.Register(new IRegistration[1]
                    {
              Component.For<ITask>().ImplementedBy(task).Named(task.TaskName())
                    });
                }
                catch (ComponentRegistrationException ex)
                {
                    throw new TaskWithSameNameAlreadyRegistredException(task, ex);
                }
            }
        }
    }
}
