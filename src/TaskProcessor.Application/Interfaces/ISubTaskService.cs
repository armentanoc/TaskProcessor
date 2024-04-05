using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Interfaces
{
    public interface ISubTaskService
    {
        IEnumerable<SubTaskEntity> GetAllSubTasks();
        SubTaskEntity CreateSubTask(int taskId);
        void Update(SubTaskEntity subTask);
    }
}
