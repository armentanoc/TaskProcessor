using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    public class SubTaskService
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
    }
}
