using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    public class SubTaskService : ITaskManager, ITaskProcessor
    {
        private readonly IRepository<SubTaskEntity> _subTaskEntityRepository;

        public SubTaskService(IRepository<SubTaskEntity> taskEntityRepository)
        {
            _subTaskEntityRepository = taskEntityRepository;
        }

        public IEnumerable<SubTaskEntity> GetAllSubTasks()
        {
            return _subTaskEntityRepository.GetAll();
        }

        public SubTaskEntity CreateSubTask(int taskId)
        {
            try
            {
                SubTaskEntity subTask = new SubTaskEntity(taskId);
                _subTaskEntityRepository.Add(subTask);
                return subTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                return null;
            }
        }

        public void Update(SubTaskEntity subTask)
        {
            try
            {
                _subTaskEntityRepository.Update(subTask);
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
