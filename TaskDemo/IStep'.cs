using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public interface IStep<in TWorkItem> : IStep
    {
        Execution ContinueWith(TWorkItem workItem);

        void Execute(TWorkItem workItem, ITaskExecutionContext context);
    }
}
