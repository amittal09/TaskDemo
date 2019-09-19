using System;
namespace TaskDemo
{
    public class IndexCategoriesStep : Step<CategoryWorkItem>
    {
        public override string Description => throw new System.NotImplementedException();

        public override void Execute(CategoryWorkItem workItem, ITaskExecutionContext context)
        {
            Console.WriteLine("IndexCategoriesStep");
        }
    }
}