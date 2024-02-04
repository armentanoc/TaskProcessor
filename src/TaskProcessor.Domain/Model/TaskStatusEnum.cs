namespace TaskProcessor.Domain.Model
{
    public enum TaskStatusEnum
    {
        Created = 1,
        Scheduled = 2,
        InProgress = 3,
        Paused = 4,
        Completed = 5,
        Cancelled = 6
    }
}
