using TaskProcessor.Domain.Model;

namespace TaskProcessor.Domain.Interfaces
{
    public interface IStatusCheckable
    {
        TaskStatusEnum Status { get; set; }
    }
}
