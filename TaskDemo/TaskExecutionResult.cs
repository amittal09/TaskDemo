using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public class TaskExecutionResult
    {
        public TaskExecutionResult(string[] output)
        {
            this.Output = output ?? new string[0];
        }

        public string[] Output { get; private set; }
    }
}
