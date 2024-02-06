
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    public class TaskService
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

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            return await _taskEntityRepository.GetAllAsync();
        }

        public IEnumerable<TaskEntity> GetAllTasksByPriorityAndNumberOfSubTasks()
        {
                return _taskEntityRepository.GetAll()
                .Where(task => task.Status != TaskStatusEnum.Completed && task.Status != TaskStatusEnum.Cancelled)
                .OrderBy(task => task.Priority)
                .ThenBy(task => task.TotalSubTasks);
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
                Console.WriteLine(ex.Message, ex.StackTrace);
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
                Console.WriteLine(ex.Message, ex.StackTrace);
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
                Console.WriteLine(ex.Message, ex.StackTrace);
            }
        }

        public TaskEntity GetTaskById(int id)
        {
            return _taskEntityRepository.GetById(id);
        }
    }
}
