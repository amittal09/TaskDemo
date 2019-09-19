using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public interface ITaskFactory
    {
        bool Exists(string name);

        ITask Get<TTask>() where TTask : class, ITask;

        ITask Get(string name);

        bool TryGet(string name, out ITask task);

        ITask[] GetAll();
    }
}
