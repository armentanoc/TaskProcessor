
using TaskProcessor.Domain.Interfaces;

namespace TaskProcessor.Domain.Model
{
    public class SubTaskEntity : BaseEntity, IStatusCheckable
    {
        private readonly Random random = new Random();
        public TaskStatusEnum Status { get; set; }
        public int TaskEntityId { get; set; }
        public TimeSpan Duration { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }
        private DateTime StartTime;

        public SubTaskEntity()
        {
            // Initialize properties, including navigation properties
        }
        public SubTaskEntity(int taskId)
        {
            Status = TaskStatusEnum.Created;
            Duration = GetRandomDuration();
            ElapsedTime = TimeSpan.Zero;
            TaskEntityId = taskId;
        }

        private TimeSpan GetRandomDuration()
        {
            int seconds = random.Next(3, 61); //professor quer
            //int seconds = random.Next(3, 10);
            return TimeSpan.FromSeconds(seconds);
        }

        public void UpdateElapsedTime()
        {
            ElapsedTime += TimeSpan.FromSeconds(1);
        }

        public override string ToString()
        {
            string startTimeString = StartTime == default(DateTime)
                ? "NotStarted"
                : StartTime.ToString("yyyy-MM-dd HH:mm:ss");

            return $"\nSubTask Id => {Id}, " +
                   $"SubTaskStatus => {Status}, " +
                   $"TaskEntity Id => {TaskEntityId}, " +
                   $"StartTime => {startTimeString}, " +
                   $"Duration (seconds) => {Duration.TotalSeconds}, " +
                   $"ElapsedTime (seconds) => {ElapsedTime.TotalSeconds}";
        }

        public void MakeSureElapsedTimeMatchesDuration()
        {
            ElapsedTime = Duration;
        }

        public void SetStatusToComplete()
        {
            Status = TaskStatusEnum.Completed;
        }

        public void SetStatusToInProgess()
        {
            Status = TaskStatusEnum.InProgress;
        }
    }
}
