
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    public class SubTaskService : IService<SubTaskEntity>
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
                ServiceHelper.LogError(ex);
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
                ServiceHelper.LogError(ex);
            }
        }
    }
}
