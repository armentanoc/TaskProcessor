using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    public class TaskService : ITaskManager, ITaskProcessor
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
        public Task Start(int idTask)
        {
            throw new NotImplementedException();
        }

        public Task Pause(int idTask)
        {
            throw new NotImplementedException();
        }

        public Task Stop(int idTask)
        {
            throw new NotImplementedException();
        }

        public Task Resume(int idTask)
        {
            throw new NotImplementedException();
        }

        public Task Cancel(int idTask)
        {
            throw new NotImplementedException();
        }

        public Task<TaskEntity> Create()
        {
            throw new NotImplementedException();
        }

        public Task<TaskEntity> Get(int idTarefa)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetActiveTasks()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetInactiveTasks()
        {
            throw new NotImplementedException();
        }
    }
}
