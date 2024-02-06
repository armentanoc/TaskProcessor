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

        public async Task Run(int numberOfTasks)
        {
            DefaultData.TryInserting(numberOfTasks, _subTaskService, _taskService);

            //DisplayData<SubTaskEntity>.Display(() => _subTaskService.GetAllSubTasks());
            //DisplayData<TaskEntity>.Display(() => _taskService.GetAllTasks());

            try
            {
                int topTasksCount = 2;

                var executeTasks = _taskExecutionService.ExecuteTopTasksWithSubTasksAsync(topTasksCount);
                var displayTask = DisplayData<TaskEntity>.DisplayAsync(() => _taskService.GetAllTasksAsync());

                await Task.WhenAll(executeTasks, displayTask);

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }
        }
    }
}
