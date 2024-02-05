using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    public class TaskService : IService<TaskEntity>
    {
        private readonly IRepository<TaskEntity> _taskEntityRepository;

        public TaskService(IRepository<TaskEntity> taskEntityRepository)
        {
            _taskEntityRepository = taskEntityRepository;
        }


        public IEnumerable<TaskEntity> GetAllTasks()
        {
            return _taskEntityRepository.GetAll();

        }

        public IEnumerable<TaskEntity> GetAllTasksOrderedByPriority()
        {
            return _taskEntityRepository.GetAll()
                .Where(task => !ServiceHelper.GetExcludedStatuses().Contains(task.Status)) //tasks to exclude
                .OrderByDescending(task => task.Priority) //high, then medium, then low
                .ThenByDescending(task => task.Status) //in progress, then scheduled, then created
                .ThenBy(task => (task.TotalSubTasks - task.CompletedSubTasks)); //subtasks to execute
        }

        public void CreateTask(SubTaskService subTaskService)
        {
            try
            {
                var task = new TaskEntity();
                _taskEntityRepository.Add(task);
                CreateSubTasks(subTaskService, task);
            }
            catch (Exception ex)
            {
                ServiceHelper.LogError(ex);
            }
        }

        private void CreateSubTasks(SubTaskService subTaskService, TaskEntity task)
        {
            try
            {
                for (int i = 0; i < task.TotalSubTasks; i++)
                {
                    SubTaskEntity subTask = subTaskService.CreateSubTask(task.Id);
                    task.SubTasks.Add(subTask);
                }
            }
            catch (Exception ex)
            {
                ServiceHelper.LogError(ex);
            }
        }

        public void Update(TaskEntity task)
        {
            try
            {
                _taskEntityRepository.Update(task);
            }
            catch (Exception ex)
            {
                ServiceHelper.LogError(ex);
            }
        }
        internal static string GetTaskInformation(TaskEntity task)
        {
            return $"Id {task.Id} - Priority {task.Priority} " +
                $"\nSubtasks: {string.Join(", ", task.SubTasks.Select(subTask => subTask.Id))}";
            //$"SubTask Id: {subTask.Id}, Duration: {subTask.Duration.TotalSeconds}s"))} {GetDateTime()}";
        }

        public async Task DisplayInformationAboutAllTasksAsync()
        {
            while (!Console.KeyAvailable)
            {
                Console.Clear();
                var tasks = await GetAllTasksAsync();
                foreach (var task in tasks)
                {
                    ServiceHelper.LogProgress(task);
                }
                await Task.Delay(1000);
            }
        }

        private Task<List<TaskEntity>> GetAllTasksAsync()
        {
            var tasks = GetAllTasks().ToList();
            return Task.FromResult(tasks);
        }
    }
}
