
using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Presentation.Helpers
{
    internal class DisplayData<T>
    {
        public static void Display(Func<IEnumerable<T>> getDataFunc)
        {
            Console.WriteLine($"\nTabela {typeof(T).Name}:");
            var entities = getDataFunc.Invoke();
            foreach (var entity in entities)
            {
                Console.WriteLine(entity.ToString());
            }
        }
        private static int menuHeight;

        public static async Task DisplayAsync(int totalTasks, Func<Task<IEnumerable<TaskEntity>>> value, TaskService _taskService)
        {
            menuHeight = totalTasks * 3;
            var progressTask = DisplayProgress(value);
            var menuTask = DisplayMenu(_taskService, value);

            await Task.WhenAll(progressTask, menuTask);
        }

        private static async Task DisplayProgress(Func<Task<IEnumerable<TaskEntity>>> value)
        {
            while (true)
            {
                var entities = await value.Invoke();
                Console.SetCursorPosition(0, 0); // Set the cursor position to the top of the console
                foreach (var entity in entities)
                {
                    LogProgress(entity);
                }

                await Task.Delay(1000);
            }
        }

        private static async Task DisplayMenu(TaskService _taskService, Func<Task<IEnumerable<TaskEntity>>> value)
        {
            Console.Clear();
            while (true)
            {
                Console.SetCursorPosition(0, menuHeight);
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Pause Task");
                Console.WriteLine("Esc. Exit");

                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        await HandlePauseTaskOption(_taskService, value);
                        break;

                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid option. Press Enter to try again...");
                        break;
                }
            }
        }

        private static async Task HandlePauseTaskOption(TaskService _taskService, Func<Task<IEnumerable<TaskEntity>>> value)
        {
            Console.Clear();
            Console.SetCursorPosition(0, menuHeight);
            Console.WriteLine("\nEnter the ID of the task you want to pause:");

            int inputRow = Console.CursorTop; 

            if (int.TryParse(Console.ReadLine(), out int taskToPauseId))
            {
                var taskToPause = _taskService.GetTaskById(taskToPauseId);
                if (taskToPause != null)
                {
                    taskToPause.Pause();
                    _taskService.Update(taskToPause);

                    Console.SetCursorPosition(0, inputRow);
                    Console.Write(new string(' ', Console.WindowWidth));

                    Console.SetCursorPosition(0, inputRow);
                    Console.WriteLine($"\n\n\nTask ({taskToPause.Id}) paused. Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("\nTask not found. Press Enter to try again...");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Press Enter to try again...");
            }
        }


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
        LogWithColor(priorityColor, $"\nProgress: {task.CompletedSubTasks}/{task.TotalSubTasks} subtasks completed for Task {task.Id} (Priority: {task.Priority} / Status: {task.Status})");
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
}
}
