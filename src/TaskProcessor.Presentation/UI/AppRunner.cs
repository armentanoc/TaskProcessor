using TaskProcessor.Application.Interfaces;
using TaskProcessor.Application.Services;
using TaskProcessor.Presentation.Helpers;

namespace TaskProcessor.Presentation.UI
{
    public class AppRunner
    {
        private readonly ISubTaskService _subTaskService;
        private readonly ITaskService _taskService;
        private readonly ITaskExecutionService _taskExecutionService;
        private readonly ConsoleDisplay consoleDisplay;

        public AppRunner(ISubTaskService subTaskService, ITaskService taskService, ITaskExecutionService taskExecutionService)
        {
            _subTaskService = subTaskService;
            _taskService = taskService;
            _taskExecutionService = taskExecutionService;
            consoleDisplay = new ConsoleDisplay(_taskService);
        }

        public async Task Run(int numberOfTasksToBeGenerated, int numberOfTasksToBeExecutedAtATime)
        {

            try
            {
                DefaultData.TryInserting(numberOfTasksToBeGenerated, _subTaskService, _taskService);

                while (true)
                {
                    var executeTasks = _taskExecutionService.ExecuteTopTasksWithSubTasksAsync(numberOfTasksToBeExecutedAtATime);
                    var displayTask = consoleDisplay.DisplayAsync(numberOfTasksToBeGenerated, () => _taskService.GetAllTasksAsync());
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
