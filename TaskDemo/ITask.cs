using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public interface ITask
    {
        string Description { get; }

        IEnumerable<IStep> Steps { get; }
    }
}
