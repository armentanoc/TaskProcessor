using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Model;
using TaskProcessor.Presentation.Helpers;

namespace TaskProcessor.Presentation
{
    public class DisplayInformation
    {
        private readonly SubTaskService _subTaskService;
        private readonly TaskService _taskService;
        private readonly TaskExecutionService _taskExecutionService;

        public DisplayInformation(SubTaskService subTaskService, TaskService taskService, TaskExecutionService taskExecutionService)
        {
            _subTaskService = subTaskService;
            _taskService = taskService;
            _taskExecutionService = taskExecutionService;
        }

        public async Task Run()
        {
            DefaultData.TryInserting(_subTaskService, _taskService);

            try
            {
                await DisplayData<TaskEntity>.DisplayAsync(() => _taskService.GetAllTasksAsync());
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }
        }
    }
}
