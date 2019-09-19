﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskDemo
{
    public abstract class Task : Task<EmptyWorkItem>
    {
        protected Task()
          : base(Enumerable.Empty<IStep<EmptyWorkItem>>())
        {
        }

        public override EmptyWorkItem Start(ITaskExecutionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this.StartTask(context);
            return new EmptyWorkItem();
        }

        public abstract void StartTask(ITaskExecutionContext context);

        public static string NameOf<TTask>() where TTask : ITask
        {
            return typeof(TTask).TaskName();
        }
    }
}
