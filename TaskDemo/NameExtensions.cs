using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    public static class NameExtensions
    {
        public static string HostName(this Type host)
        {
            if (host == (Type)null)
                throw new ArgumentNullException(nameof(host));
            return host.Name;
        }

        public static string Name(this IHost host)
        {
            if (host == null)
                throw new ArgumentNullException(nameof(host));
            return host.GetType().HostName();
        }

        public static string TaskName(this Type task)
        {
            if (task == (Type)null)
                throw new ArgumentNullException(nameof(task));
            return task.Name;
        }

        public static string Name(this ITask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            return task.GetType().TaskName();
        }

        public static string StepName(this Type step)
        {
            if (step == (Type)null)
                throw new ArgumentNullException(nameof(step));
            return step.Name;
        }

        public static string Name(this IStep step)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));
            return step.GetType().StepName();
        }
    }
}
