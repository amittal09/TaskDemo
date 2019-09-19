using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public interface IApplicationContext :IDisposable
    {
        void Execute(params string[] args);

        void Execute(HostArguments args);
    }
}
