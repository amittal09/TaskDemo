using System;
using System.IO;

namespace TaskDemo
{
    public class TaskHost : IHost
    {
        private readonly ITaskFactory _factory;
        private readonly ITaskRunner _runner;
        private readonly TextWriter _textWriter;

        public TaskHost(ITaskFactory factory, ITaskRunner runner,TextWriter textWriter)
        {
            this._factory = factory;
            this._runner = runner;
            this._textWriter = textWriter;
        }

        public bool CanHandle(HostArguments args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            if (!string.IsNullOrWhiteSpace(args.Command))
                return this._factory.Exists(args.Command);
            return false;
        }

        public void Handle(HostArguments args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            ITask task = this._factory.Get(args.Command);
            this._runner.Execute(task, args.Args);
        }
        public string Description
        {
            get
            {
                return "Handles execution of Tasks.";
            }
        }
    }
}
