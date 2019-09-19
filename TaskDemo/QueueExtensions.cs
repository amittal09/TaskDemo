using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDemo
{
    internal static class QueueExtensions
    {
        public static T SafePeek<T>(this Queue<T> queue, T valueIfEmpty)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));
            if (queue.Count <= 0)
                return valueIfEmpty;
            return queue.Peek();
        }
    }
}
