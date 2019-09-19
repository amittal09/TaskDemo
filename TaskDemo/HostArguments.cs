using System.Collections.Generic;

namespace TaskDemo
{
    public class HostArguments
    {
        public HostArguments(string command, KeyValuePair<string, string>[] commandArgs, KeyValuePair<string, string>[] args)
        {
            this.Command = command;
            this.CommandArgs = new Arguments("-", commandArgs);
            this.Args = new Arguments(string.Empty, args);
        }

        public string Command { get; }

        public Arguments CommandArgs { get; }

        public Arguments Args { get; }

        public override string ToString()
        {
            return string.Format("{0} [CommandArgs = {1}] [Args = {2}]", (object)this.Command, (object)this.CommandArgs, (object)this.Args);
        }
    }
}