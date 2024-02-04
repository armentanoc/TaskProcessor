
namespace TaskProcessor.Domain.Model
{
    public class SubTaskEntity : BaseEntity
    {
        private readonly Random random = new Random();
        public TaskStatusEnum Status { get; set; }
        public int TaskId { get; set; }
        public TimeSpan Duration { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }
        private DateTime StartTime;

        public SubTaskEntity(int taskId)
        {
            Status = TaskStatusEnum.Created;
            Duration = GetRandomDuration();
            ElapsedTime = TimeSpan.Zero;
            TaskId = taskId;
        }

        private TimeSpan GetRandomDuration()
        {
            //int seconds = random.Next(3, 61); //professor pediu
            int seconds = random.Next(3, 5); //teste
            return TimeSpan.FromSeconds(seconds);
        }

        public void UpdateElapsedTime()
        {
            ElapsedTime += TimeSpan.FromSeconds(1);

            if (ElapsedTime >= Duration)
            {
                ElapsedTime = Duration;
                Status = TaskStatusEnum.Completed;
            }
        }

        public void StartSubTask()
        {
            StartTime = DateTime.Now;
            Status = TaskStatusEnum.InProgress;
        }

        public override string ToString()
        {
            string startTimeString = StartTime == default(DateTime)
                ? "NotStarted"
                : StartTime.ToString("yyyy-MM-dd HH:mm:ss");

            return $"\nSubTask Id => {Id}, " +
                   $"SubTaskStatus => {Status}, " +
                   $"TaskEntity Id => {TaskId}, " +
                   $"StartTime => {startTimeString}, " +
                   $"Duration (seconds) => {Duration.TotalSeconds}, " +
                   $"ElapsedTime (seconds) => {ElapsedTime.TotalSeconds}";
        }
    }
}
