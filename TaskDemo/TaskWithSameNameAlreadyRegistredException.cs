using System;
using System.Runtime.Serialization;

namespace TaskDemo
{
    [Serializable]
    public class TaskWithSameNameAlreadyRegistredException : Exception
    {
        public TaskWithSameNameAlreadyRegistredException()
        {
        }

        internal TaskWithSameNameAlreadyRegistredException(Type task)
          : base(TaskWithSameNameAlreadyRegistredException.BuildMessage(task))
        {
        }

        internal TaskWithSameNameAlreadyRegistredException(Type task, Exception innerException)
          : base(TaskWithSameNameAlreadyRegistredException.BuildMessage(task), innerException)
        {
        }

        protected TaskWithSameNameAlreadyRegistredException(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }

        private static string BuildMessage(Type task)
        {
            if (task == (Type)null)
                throw new ArgumentNullException(nameof(task));
            return string.Format("Unable to register Task '{0}'. A task with the same name '{1}' is already registred. \r\nConsider running the \"WriteDocumentationTask\" to get a text-output of available tasks.", (object)task.FullName, (object)task.TaskName());
        }
    }
}
