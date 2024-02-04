
using TaskProcessor.Domain.Model.Enums;

namespace TaskProcessor.Domain.Model
{
    public class TaskEntity : BaseEntity
    {
        private Random random;
        public TaskPriority Priority { get; set; }
        public TaskEntityStatus Status { get; set; }
        public int TotalSubTasks { get; set; }
        public int CompletedSubTasks { get; set; }
        public List<SubTaskEntity> SubTasks { get; set; }
        public TaskEntity()
        {
            Status = TaskEntityStatus.Created;
            Priority = GetRandomTaskPriority();
            TotalSubTasks = GetRandomTotalSubTasks();
            CompletedSubTasks = 0;
            SubTasks = new List<SubTaskEntity>();
        }
        public bool IsTaskCompleted()
        {
            return CompletedSubTasks == TotalSubTasks;
        }
        private int GetRandomTotalSubTasks()
        {
            random = new Random();
            //return random.Next(10, 101); //professor pediu
            return random.Next(1, 5); //teste
        }

        private TaskPriority GetRandomTaskPriority()
        {
            Array values = Enum.GetValues(typeof(TaskPriority));
            random = new Random();
            TaskPriority randomPriority = (TaskPriority)values.GetValue(random.Next(values.Length));
            return randomPriority;
        }
        public override string ToString()
        {
            string subTasksString = SubTasks.Any() ? $"SubTasks: {string.Join(", ", SubTasks)}" : "No SubTasks";
            int totalDurationSeconds = SubTasks.Sum(subTask => (int)subTask.Duration.TotalSeconds);
            int totalElapsedTimeSeconds = SubTasks.Sum(subTask => (int)subTask.ElapsedTime.TotalSeconds);

            return $"\nTask Id: {Id}" +
                   $"\nTaskEntityStatus: {Status}" +
                   $"\nPriority: {Priority}" +
                   $"\nTotalSubTasks: {TotalSubTasks}" +
                   $"\nCompletedSubTasks: {CompletedSubTasks}" +
                   $"\nTotal Duration (seconds): {totalDurationSeconds}" +
                   $"\nTotal Elapsed Time (seconds): {totalElapsedTimeSeconds}" +
                   $"\n\n{subTasksString}";
        }
    }
}
