using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Model;
using TaskProcessor.Presentation.Helpers;

namespace TaskProcessor.Presentation
{
    public class AppRunner
    {
        private readonly SubTaskService _subTaskService;
        private readonly TaskService _taskService;

        public AppRunner(SubTaskService subTaskService, TaskService taskService)
        {
            _subTaskService = subTaskService;
            _taskService = taskService;
        }

        public void Run()
        {
            DefaultData.TryInserting(_subTaskService, _taskService);

            //DisplayData<SubTaskEntity>.Display(() => _subTaskService.GetAllSubTasks());
            //DisplayData<TaskEntity>.Display(() => _taskService.GetAllTasks());

            var tasksByPriority = _taskService.GetAllTasksByPriorityAndNumberOfSubTasks();
            DisplayData<TaskEntity>.Display(() => tasksByPriority);

            //Create.Customer(_subTaskService);

            //Console.WriteLine("\nWorks fine. Press any key to exit...");
            Console.ReadKey();
        }
    }
}
