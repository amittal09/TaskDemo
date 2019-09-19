using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public abstract class Task<TWorkItem> : ITask<TWorkItem>, ITask
    {
        private readonly IEnumerable<IStep<TWorkItem>> _steps;

        protected Task(IEnumerable<IStep<TWorkItem>> steps)
        {
            this._steps = steps ?? Enumerable.Empty<IStep<TWorkItem>>();
        }

        [JsonProperty(Order = 1)]
        public string Name
        {
            get
            {
                return this.Name();
            }
        }

        [JsonProperty(Order = 2)]
        public abstract string Description { get; }

        [JsonProperty(Order = 3)]
        public IEnumerable<IStep<TWorkItem>> Steps
        {
            get
            {
                return this.Steps1;
            }
        }

        IEnumerable<IStep> ITask.Steps
        {
            get
            {
                return (IEnumerable<IStep>)this.Steps;
            }
        }

        public IEnumerable<IStep<TWorkItem>> Steps1 => Steps2;

        public IEnumerable<IStep<TWorkItem>> Steps2 => _steps;

        public IEnumerable<IStep<TWorkItem>> Steps3 => _steps;

        public abstract TWorkItem Start(ITaskExecutionContext context);

        public virtual void End(TWorkItem workItem, ITaskExecutionContext context)
        {
        }
    }
}
