using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public sealed class TaskConfiguration<TWorkItem> : TaskConfiguration
    {
        internal TaskConfiguration(Type task)
          : base(task)
        {
        }

        public TaskConfiguration<TWorkItem> Step<TStep>() where TStep : IStep<TWorkItem>
        {
            this.Steps.Add(typeof(TStep));
            return this;
        }

        public TaskConfiguration<TWorkItem> Clear()
        {
            this.Steps.Clear();
            return this;
        }

        public TaskConfiguration<TWorkItem> Remove<TStep>() where TStep : IStep<TWorkItem>
        {
            this.Steps.RemoveAll((Predicate<Type>)(x => x == typeof(TStep)));
            return this;
        }

        internal override IWindsorInstaller GetInstaller()
        {
            return (IWindsorInstaller)new TaskInstaller<TWorkItem>(this.Task, (IEnumerable<Type>)this.Steps);
        }
    }
}
