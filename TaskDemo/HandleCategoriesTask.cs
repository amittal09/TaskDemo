using System.Collections.Generic;

namespace TaskDemo
{
    public class HandleCategoriesTask : Task<CategoryWorkItem>
    {
        public override string Description => "HandleCategoriesTask";
        public HandleCategoriesTask(IEnumerable<IStep<CategoryWorkItem>> steps) : base(steps)
        {
        }
        public override CategoryWorkItem Start(ITaskExecutionContext context)
        {
            return new CategoryWorkItem();
        }
    }
}