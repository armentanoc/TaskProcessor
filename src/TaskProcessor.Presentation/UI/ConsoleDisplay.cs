using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Model;

internal class ConsoleDisplay
{
    private static int menuHeight;

    public static async Task DisplayAsync(int totalTasks, Func<Task<IEnumerable<TaskEntity>>> value, TaskService taskService)
    {
        menuHeight = totalTasks * 3;
        var progressTask = DisplayData<TaskEntity>.DisplayProgress(value);
        var menuTask = DisplayMenu(taskService, value);

        await Task.WhenAll(progressTask, menuTask);
    }

    private static async Task DisplayMenu(TaskService taskService, Func<Task<IEnumerable<TaskEntity>>> value)
    {
        Console.Clear();
        while (true)
        {
            Console.SetCursorPosition(0, menuHeight);
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Pause Task Yet To Be Started");
            Console.WriteLine("2. Restart Paused Task");
            Console.WriteLine("Esc. Exit");

            var key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    await HandlePauseTaskOption(taskService, value);
                    break;

                case ConsoleKey.D2:
                    await RestartPausedTaskOption(taskService, value);
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

    private static async Task RestartPausedTaskOption(TaskService taskService, Func<Task<IEnumerable<TaskEntity>>> value)
    {
        Console.Clear();
        Console.SetCursorPosition(0, menuHeight);
        Console.WriteLine("\nEnter the ID of the paused task you want to restart:");

        int inputRow = Console.CursorTop;

        if (int.TryParse(Console.ReadLine(), out int pausedTaskId))
        {
            var pausedTask = taskService.GetTaskById(pausedTaskId);
            if (pausedTask != null && pausedTask.Status == TaskStatusEnum.Paused)
            {
                pausedTask.Resume(); 
                taskService.Update(pausedTask);

                Console.SetCursorPosition(0, inputRow);
                Console.Write(new string(' ', Console.WindowWidth));

                Console.SetCursorPosition(0, inputRow);
                Console.WriteLine($"\n\n\nTask ({pausedTask.Id}) restarted. Press Enter to continue...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("\nPaused task not found or not in a paused state. Press Enter to try again...");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid input. Press Enter to try again...");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.Clear();
    }

    private static async Task HandlePauseTaskOption(TaskService taskService, Func<Task<IEnumerable<TaskEntity>>> value)
    {
        Console.Clear();
        Console.SetCursorPosition(0, menuHeight);
        Console.WriteLine("\nEnter the ID of the task you want to pause:");

        int inputRow = Console.CursorTop;

        if (int.TryParse(Console.ReadLine(), out int taskToPauseId))
        {
            var taskToPause = taskService.GetTaskById(taskToPauseId);
            if (taskToPause != null && taskToPause.Status != TaskStatusEnum.Paused)
            {
                taskToPause.Pause();
                taskService.Update(taskToPause);

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

        Console.WriteLine("Press Enter to continue...");
        Console.Clear();
    }
}
