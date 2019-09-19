
using System;
namespace TaskDemo
{
    internal class SaveNewCategoryIdsInDbStep : Step<CategoryWorkItem>
    {
        public override string Description => throw new System.NotImplementedException();

        public override void Execute(CategoryWorkItem workItem, ITaskExecutionContext context)
        {
            Console.WriteLine(workItem.Name);
            Console.WriteLine("SaveNewCategoryIdsInDbStep");
        }
    }
}