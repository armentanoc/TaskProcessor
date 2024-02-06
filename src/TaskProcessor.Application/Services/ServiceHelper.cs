
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    internal class ServiceHelper
    {

        private static object _fileLock = new object();
        public static void Write(string message)
        {
            Console.WriteLine(message);
        }
        public static void LogWithColor(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Write(message);
            Console.ResetColor();
        }
        public static void LogProgress(TaskEntity task)
        {
            var completed = task.CompletedSubTasks;
            var total = task.TotalSubTasks;
            var priorityColor = GetPriorityColor(task.Priority);
            LogWithColor(priorityColor, $"\nProgress: {task.CompletedSubTasks}/{task.TotalSubTasks} subtasks completed for Task {task.Id} (Priority: {task.Priority})");
            LogProgressBar(completed, total);
        }
        private static ConsoleColor GetPriorityColor(TaskPriorityEnum priority) =>
            priority switch
            {
                TaskPriorityEnum.High => ConsoleColor.Red,
                TaskPriorityEnum.Medium => ConsoleColor.Yellow,
                TaskPriorityEnum.Low => ConsoleColor.Blue,
                _ => ConsoleColor.White // Default color if priority is unknown
            };
        public static void LogProgressBar(int completed, int total)
        {
            Console.Write("[");

            int progressBarLength = 20;
            int progress = (int)Math.Round((double)completed / total * progressBarLength);

            for (int i = 0; i < progressBarLength; i++)
            {
                if (i < progress)
                    Console.Write("█");
                else
                    Console.Write("░");
            }

            double percentage = (double)completed / total * 100;
            Console.Write($"] {percentage:F1}%\n\r");
            Console.SetCursorPosition(0, Console.CursorTop);
        }
        private static void Log(string message)
        {
            string filePath = "log_task_execution_service.txt";

            lock (_fileLock)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(message);
                }
            }
        }

        internal static void LogStartTask(TaskEntity task)
        {
            Log($"\n[STARTED] Executing Task: {ServiceHelper.GetTaskInformation(task)}");
        }

        internal static void LogCompleteTask(TaskEntity task)
        {
            Log($"[COMPLETED] Task Completed: {ServiceHelper.GetTaskInformation(task)}");
        }

        internal static string GetDateTime()
        {
            return $"At {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}";
        }
        internal static string GetSubTaskInformation(SubTaskEntity subTask)
        {
            return
                $"[INFO] {subTask.Id} {GetDateTime()}" +
                $" Duration: {subTask.Duration}" +
                $" ElapsedTime: {subTask.ElapsedTime}";
        }
        internal static string GetTaskInformation(TaskEntity task)
        {
            return $"[INFO] Id {task.Id} - Priority {task.Priority}" +
                $"\nSubtasks: {string.Join(", ", task.SubTasks.Select(subTask => $"SubTask Id: {subTask.Id}, Duration: {subTask.Duration.TotalSeconds}s"))} {GetDateTime()}";
        }
        internal static void LogUpdateSubTask(SubTaskEntity subTask)
        {
            Log($"\n[UPDATE] SubTask Updated: {ServiceHelper.GetSubTaskInformation(subTask)}");
        }

        internal static void LogStartSubTask(SubTaskEntity subTask)
        {
            Log($"\n[STARTED] Executing SubTask: {ServiceHelper.GetSubTaskInformation(subTask)}");
        }

        internal static void LogTaskProgress(TaskEntity parentTask)
        {
            Log($"\n[TASK PROGRESS] Progress: {parentTask.CompletedSubTasks}/{parentTask.TotalSubTasks} subtasks completed for Task {parentTask.Id}");
        }

        internal static void LogCompleteSubTask(SubTaskEntity subTask)
        {
            Log($"\n[COMPLETED] SubTask Completed: {ServiceHelper.GetSubTaskInformation(subTask)}");
        }
    }
}
