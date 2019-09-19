namespace TaskDemo
{
    public abstract class Step<TWorkItem> : IStep<TWorkItem>, IStep
    {
        public abstract string Description { get; }

        public virtual Execution ContinueWith(TWorkItem workItem)
        {
            return Execution.Execute;
        }

        public abstract void Execute(TWorkItem workItem, ITaskExecutionContext context);
    }
}