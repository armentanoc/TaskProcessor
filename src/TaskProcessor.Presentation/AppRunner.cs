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

        public async Task Run(int numberOfTasksToBeGenerated, int numberOfTasksToBeExecutedAtATime)
        {
            DefaultData.TryInserting(numberOfTasksToBeGenerated, _subTaskService, _taskService);

            try
            {
                while (true)
                {
                    var executeTasks = _taskExecutionService.ExecuteTopTasksWithSubTasksAsync(numberOfTasksToBeExecutedAtATime);
                    var displayTask = DisplayData<TaskEntity>.DisplayAsync(numberOfTasksToBeGenerated, () => _taskService.GetAllTasksAsync(), _taskService);
                    await Task.WhenAll(executeTasks, displayTask);

                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message} - {ex.StackTrace}");
            }
        }
    }
}
