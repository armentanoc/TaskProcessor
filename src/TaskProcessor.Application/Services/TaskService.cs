
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Model;
using TaskProcessor.Infra.Interfaces;

namespace TaskProcessor.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskEntityRepository _taskEntityRepository;
        private readonly ISubTaskService _subTaskService;
        public TaskService(ITaskEntityRepository taskEntityRepository, ISubTaskService subTaskService)
        {
            _taskEntityRepository = taskEntityRepository;
            _subTaskService = subTaskService;
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

        public void CreateTask()
        {
            try
            {
                var task = new TaskEntity();
                _taskEntityRepository.Add(task);
                CreateSubTasks(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
            }
        }

        public void CreateSubTasks(TaskEntity task)
        {
            try
            {
                for (int i = 0; i < task.TotalSubTasks; i++)
                {
                    SubTaskEntity subTask = _subTaskService.CreateSubTask(task.Id);
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
