
using TaskProcessor.Domain.Model.Enums;

namespace TaskProcessor.Domain.Model
{
    public class SubTaskEntity : BaseEntity
    {
        public SubTaskEntityStatus Status { get; set; }
        public int TaskId { get; set; }
        public TimeSpan Duration { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }
        private DateTime StartTime;

        public SubTaskEntity(int taskId)
        {
            Status = SubTaskEntityStatus.Created;
            Duration = GetRandomDuration();
            TaskId = taskId;
        }

        private TimeSpan GetRandomDuration()
        {
            Random random = new Random();
            //int seconds = random.Next(3, 61); //professor pediu
            int seconds = random.Next(3, 5); //teste
            return TimeSpan.FromSeconds(seconds);
        }

        public void UpdateElapsedTime()
        {
            ElapsedTime = DateTime.Now - StartTime;
        }

        public void StartSubTask()
        {
            StartTime = DateTime.Now;
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
