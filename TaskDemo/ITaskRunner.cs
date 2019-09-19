using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public interface ITaskRunner
    {
        TaskExecutionResult Execute(ITask task, Arguments arguments = null);
    }
}
