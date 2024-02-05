using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Model;
using TaskProcessor.Presentation.Helpers;

namespace TaskProcessor.Presentation
{
    public class AppRunner
    {
        private readonly SubTaskService _subTaskService;
        private readonly TaskService _taskService;
        private readonly TaskExecutionService _taskExecutionService;

        public AppRunner(SubTaskService subTaskService, TaskService taskService, TaskExecutionService taskExecutionService)
        {
            _subTaskService = subTaskService;
            _taskService = taskService;
            _taskExecutionService = taskExecutionService;
        }

        public async Task Run()
        {
            DefaultData.TryInserting(_subTaskService, _taskService);

            //DisplayData<SubTaskEntity>.Display(() => _subTaskService.GetAllSubTasks());
            //DisplayData<TaskEntity>.Display(() => _taskService.GetAllTasks());
            //DisplayData<TaskEntity>.Display(() => _taskService.GetAllTasksOrderedByPriority());
            try
            {
                int concurrentTasksCount = 2;

                Task displayTask = _taskService.DisplayInformationAboutAllTasksAsync();
                Task executeTask = _taskExecutionService.ExecuteTopTasksWithSubTasksAsync(concurrentTasksCount);

                await Task.WhenAll(displayTask, executeTask);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }

            //Create.Customer(_subTaskService);

            //Console.WriteLine("\nWorks fine. Press any key to exit...");
        }
    }
}
