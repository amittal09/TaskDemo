using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TaskDemo
{
    public class TasksConfiguration : IInitializable<IWindsorContainer>
    {
        public ApplicationConfiguration Application { get; private set; }
        private readonly List<Assembly> _scan;
        private readonly List<Type> _simpleTasks;
        private readonly List<Type> _removeTasks;
        private readonly List<TaskConfiguration> _complexTasks;

        internal TasksConfiguration(ApplicationConfiguration application)
        {
            if (application == null)
                throw new ArgumentNullException(nameof(application));
            this.Application = application;
            this._scan = new List<Assembly>();
            this._simpleTasks = new List<Type>();
            this._removeTasks = new List<Type>();
            this._complexTasks = new List<TaskConfiguration>();
            this.AddFromAssemblyOfThis<TasksConfiguration>();
        }



        public TasksConfiguration AddFromAssemblyOfThis<T>()
        {
            this._scan.Add(typeof(T).Assembly);
            return this;
        }

        public TasksConfiguration Task<TTask, TWorkItem>(Action<TaskConfiguration<TWorkItem>> task = null) where TTask : Task<TWorkItem>
        {
            TaskConfiguration<TWorkItem> taskConfiguration = new TaskConfiguration<TWorkItem>(typeof(TTask));
            if (task != null)
                task(taskConfiguration);
            if (_complexTasks.Contains(taskConfiguration))
                throw new InvalidOperationException(string.Format("Task '{0}' has already been added.", (object)taskConfiguration.Task.FullName));
            _complexTasks.Add(taskConfiguration);
            return this;
        }

        public TasksConfiguration Task<TTask>() where TTask : Task
        {
            this._simpleTasks.Add(typeof(TTask));
            return this;
        }

        public TasksConfiguration Remove<TTask>() where TTask : Task
        {
            this._removeTasks.Add(typeof(TTask));
            return this;
        }

        void IInitializable<IWindsorContainer>.Initialize(IWindsorContainer container)
        {
            container.Install(new IWindsorInstaller[1]
            {
        (IWindsorInstaller) new TaskInstaller(this._scan.ToArray(), this._simpleTasks.ToArray(), this._removeTasks.ToArray())
            });
            foreach (TaskConfiguration taskConfiguration in this._complexTasks.Distinct<TaskConfiguration>())
                container.Install(new IWindsorInstaller[1]
                {
          taskConfiguration.GetInstaller()
                });
            container.Install(new IWindsorInstaller[1]
            {
        (IWindsorInstaller) new TaskFactoryInstaller()
            });
        }

        public TasksConfiguration Clear()
        {
            this._scan.Clear();
            this._simpleTasks.Clear();
            this._removeTasks.Clear();
            return this;
        }
    }
}