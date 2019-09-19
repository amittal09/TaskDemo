using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskDemo
{
    public class TaskInstaller<TWorkItem> : IWindsorInstaller
    {
        private readonly Type _task;
        private readonly IEnumerable<Type> _steps;

        public TaskInstaller(Type task, IEnumerable<Type> steps)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (steps == null)
                throw new ArgumentNullException(nameof(steps));
            this._task = task;
            this._steps = steps;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {
                container.Register(new IRegistration[1] { Component.For(typeof(ITask)).ImplementedBy(this._task).Named(this._task.TaskName()) });
            }
            catch (ComponentRegistrationException ex)
            {
                throw new TaskWithSameNameAlreadyRegistredException(_task, ex);
            }
            List<string> stringList = new List<string>();
            foreach (Type step in _steps)
            {
                string str = string.Format("{0}.{1}.{2}", _task.TaskName(), step.StepName(), Guid.NewGuid().ToString("N"));
                container.Register(new IRegistration[1]
                {
            Component.For<IStep<TWorkItem>>().ImplementedBy(step).Named(str)
                });
                stringList.Add(str);
            }
            container.Kernel.Resolver.AddSubResolver(new TaskStepsResolver(container.Kernel, _task, stringList.ToArray()));
        }

        private class TaskStepsResolver : ISubDependencyResolver
        {
            private readonly IKernel _kernel;
            private readonly Type _task;
            private readonly string[] _stepNames;

            public TaskStepsResolver(IKernel kernel, Type task, string[] stepNames)
            {
                if (kernel == null)
                    throw new ArgumentNullException(nameof(kernel));
                if (stepNames == null)
                    throw new ArgumentNullException(nameof(stepNames));
                this._kernel = kernel;
                this._task = task;
                this._stepNames = stepNames;
            }

            public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
            {
                if (model.Implementation == _task)
                    return dependency.TargetItemType == typeof(IEnumerable<IStep<TWorkItem>>);
                return false;
            }

            public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
            {
                return ((IEnumerable<string>)_stepNames).Select(x => _kernel.Resolve<IStep<TWorkItem>>(x)).ToArray();
            }
        }
    }
}
