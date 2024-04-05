
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Model;
using TaskProcessor.Infra.Interfaces;

namespace TaskProcessor.Application.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly ISubTaskEntityRepository _subTaskEntityRepository;

        public SubTaskService(ISubTaskEntityRepository taskEntityRepository)
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
    }
}
