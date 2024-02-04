namespace TaskProcessor.Domain.Model.Enums
{
    public enum SubTaskEntityStatus
    {
        Created = 1,
        Scheduled = 2,
        InProgress = 3,
        Paused = 4,
        Completed = 5,
        Canceled = 6
    }
}
