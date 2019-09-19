using System;

namespace TaskDemo
{
    internal class GetCategoriesFromCsvStep : Step<CategoryWorkItem>
    {
        public override string Description => "GetCategoriesFromCsvStep";

        public override void Execute(CategoryWorkItem workItem, ITaskExecutionContext context)
        {
            Console.WriteLine(workItem.Name);
            workItem.Name = "Vikram";
            Console.WriteLine("GetCategoriesFromCsvStep");
        }
    }
}