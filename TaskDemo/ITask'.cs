using System.Collections.Generic;

namespace TaskDemo
{
    public interface ITask<TWorkItem> : ITask
    {
        TWorkItem Start(ITaskExecutionContext context);

        void End(TWorkItem workItem, ITaskExecutionContext context);

        IEnumerable<IStep<TWorkItem>> Steps { get; }
    }
}
