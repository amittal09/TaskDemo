using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;

namespace TaskDemo
{
    public abstract class TaskConfiguration : IEquatable<TaskConfiguration>
    {
        protected TaskConfiguration(Type task)
        {
            if (task == (Type)null)
                throw new ArgumentNullException(nameof(task));
            this.Task = task;
            this.Steps = new List<Type>();
        }

        internal Type Task { get; }

        protected List<Type> Steps { get; }

        internal abstract IWindsorInstaller GetInstaller();

        public bool Equals(TaskConfiguration other)
        {
            if (other == null)
                return false;
            if (this == other)
                return true;
            return this.Task == other.Task;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this == obj)
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((TaskConfiguration)obj);
        }

        public override int GetHashCode()
        {
            return this.Task.GetHashCode();
        }
    }
}