using System;

namespace TaskDemo
{
    public class TaskExecutionContext : ITaskExecutionContext
    {
        public TaskExecutionContext(Arguments arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));
            this.Arguments = arguments;
        }

        public Arguments Arguments { get; }
    }
}