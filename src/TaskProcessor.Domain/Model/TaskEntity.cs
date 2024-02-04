namespace TaskProcessor.Domain.Model
{
    public class TaskEntity : BaseEntity
    {
        private readonly Random random = new Random();
        public TaskPriorityEnum Priority { get; set; }
        public TaskStatusEnum Status { get; set; }
        public int TotalSubTasks { get; set; }
        public int CompletedSubTasks { get; set; }
        public List<SubTaskEntity> SubTasks { get; set; }
        public TaskEntity()
        {
            Status = TaskStatusEnum.Created;
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
            //return random.Next(10, 101); //professor pediu
            return random.Next(1, 5); //teste
        }

        private TaskPriorityEnum GetRandomTaskPriority()
        {
            Array values = Enum.GetValues(typeof(TaskPriorityEnum));
            TaskPriorityEnum randomPriority = (TaskPriorityEnum)values.GetValue(random.Next(values.Length));
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
