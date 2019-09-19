using System;
using System.Collections.Generic;
using System.IO;
using Vertica.Utilities;

namespace TaskDemo
{
    public class TaskRunner : ITaskRunner
    {
        private readonly TextWriter _outputter;

        public TaskRunner(TextWriter outputter)
        {
            this._outputter = outputter;
        }

        public TaskExecutionResult Execute(ITask task, Arguments arguments = null)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            dynamic obj = task;
            return ExecuteInternal(obj, arguments);
        }

        private TaskExecutionResult ExecuteInternal<TWorkItem>(ITask<TWorkItem> task, Arguments arguments)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));
            List<string> output = new List<string>();
            Action<string> message1 = message =>
            {
                message = string.Format("[{0:HH:mm:ss}] {1}", (object)Time.Now, (object)message);
                this._outputter.WriteLine(message);
                output.Add(message);
            };

            TWorkItem workItem;
            try
            {
                workItem = task.Start((ITaskExecutionContext)new TaskExecutionContext(arguments));
            }
            catch (Exception ex)
            {
                throw new Exception("Starting task failed.", ex);
            }
            try
            {
                foreach (IStep<TWorkItem> step in task.Steps)
                {
                    switch (step.ContinueWith(workItem))
                    {
                        case Execution.StepOut:
                            goto label_20;
                        case Execution.StepOver:
                            continue;
                        default:

                            try
                            {
                                step.Execute(workItem, (ITaskExecutionContext)new TaskExecutionContext(arguments));
                                continue;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Step '{0}' failed.",step.Name()), ex);
                            }

                    }
                }
            label_20:
                try
                {
                    task.End(workItem, (ITaskExecutionContext)new TaskExecutionContext(arguments));
                }
                catch (Exception ex)
                {
                    throw new Exception("Ending task failed.", ex);
                }
            }
            finally
            {
               // workItem.DisposeIfDisposable();
            }

            return new TaskExecutionResult(output.ToArray());
        }
    }
}
