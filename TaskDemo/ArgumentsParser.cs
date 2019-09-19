using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskDemo
{
    public class ArgumentsParser : IArgumentsParser
    {
        public HostArguments Parse(string[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));
            string command = ((IEnumerable<string>)arguments).FirstOrDefault<string>() ?? string.Empty;
            if (command.TrimStart().StartsWith("-"))
                command = string.Empty;
            List<KeyValuePair<string, string>> keyValuePairList1 = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> keyValuePairList2 = new List<KeyValuePair<string, string>>();
            Queue<string> remaining = new Queue<string>(((IEnumerable<string>)arguments).Skip<string>(string.IsNullOrWhiteSpace(command) ? 0 : 1));
            while (remaining.Count > 0)
            {
                string key = remaining.Dequeue();
                if (key.StartsWith("-"))
                {
                    if (key.Length > 1)
                        keyValuePairList1.Add(ArgumentsParser.Map(key.Substring(1), remaining));
                }
                else
                    keyValuePairList2.Add(ArgumentsParser.Map(key, remaining));
            }
            return new HostArguments(command, keyValuePairList1.ToArray(), keyValuePairList2.ToArray());
        }

        private static KeyValuePair<string, string> Map(string key, Queue<string> remaining)
        {
            string str = (string)null;
            int length = key.IndexOf(":", StringComparison.InvariantCulture);
            if (length > 0)
            {
                str = key.Substring(length + 1);
                key = key.Substring(0, length);
            }
            else if (remaining.SafePeek<string>(string.Empty).StartsWith(":"))
                str = remaining.Dequeue().Substring(1);
            if (str != null && str.Length == 0)
                str = remaining.Count > 0 ? remaining.Dequeue() : (string)null;
            return new KeyValuePair<string, string>(key, str);
        }
    }
}
