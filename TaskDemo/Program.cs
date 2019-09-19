namespace TaskDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = ApplicationContext.Create(configuraiton => configuraiton.Tasks(tasks => RegisterTasks(configuraiton)));

            context.Execute(nameof(HandleCategoriesTask));
        }
        public static ApplicationConfiguration RegisterTasks(ApplicationConfiguration configuration)
        {
            configuration.Tasks(tasks => tasks.Task<HandleCategoriesTask, CategoryWorkItem>(task => task
                          .Step<GetCategoriesFromCsvStep>()
                          .Step<SaveNewCategoryIdsInDbStep>()
                          .Step<AssignIdToCategoryStep>()
                          .Step<IndexCategoriesStep>()
                    ));
            return configuration;
        }

    }
}
