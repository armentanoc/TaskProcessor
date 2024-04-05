using TaskProcessor.Application.Interfaces;
using TaskProcessor.Domain.Model;

internal class ConsoleDisplay
{
    private readonly ITaskService taskService;
    private static int menuHeight;

    public ConsoleDisplay(ITaskService taskService)
    {
        this.taskService = taskService;
    }

    public async Task DisplayAsync(int totalTasks, Func<Task<IEnumerable<TaskEntity>>> value)
    {
        menuHeight = totalTasks * 3;
        var progressTask = DisplayData<TaskEntity>.DisplayProgress(value);
        var menuTask = DisplayMenu(taskService, value);

        await Task.WhenAll(progressTask, menuTask);
    }

    private async Task DisplayMenu(ITaskService taskService, Func<Task<IEnumerable<TaskEntity>>> value)
    {
        while (true)
        {
            FixConsolePosition();
            Console.CursorVisible = false;
            Console.WriteLine("\nSelect an option:");
            Console.WriteLine("1. Pause Task Yet To Be Started");
            Console.WriteLine("2. Restart Paused Task");
            Console.WriteLine("Esc. Exit");

            var key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    await HandleTaskOption(value, PauseTaskAction, "PAUSE", "PAUSED");
                    break;

                case ConsoleKey.D2:
                    await HandleTaskOption(value, RestartTaskAction, "RESTART", "RESTARTED");
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

    private async Task HandleTaskOption(Func<Task<IEnumerable<TaskEntity>>> value, Action<TaskEntity> taskAction, string actionDescription, string actionResult)
    {
        FixConsolePosition();
        Console.WriteLine($"\nEnter the ID of the task you want to {actionDescription}:");

        int inputRow = Console.CursorTop;

        if (int.TryParse(Console.ReadKey().Key.ToString().Replace("D", ""), out int taskId))
        {
            FixConsolePosition();
            var task = taskService.GetTaskById(taskId);

            if (task != null)
            {
                taskAction(task);
                Console.WriteLine($"\nTask {task.Id} {actionResult}.");
                taskService.Update(task);
            }
            else
                Console.WriteLine("\nTask not found.");
        }
        else
        {
            FixConsolePosition();
            Console.WriteLine("\nInvalid input.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadLine();
        Console.Clear();
    }

    private void PauseTaskAction(TaskEntity task)
    {
        if (task.Status != TaskStatusEnum.Paused)
            task.Pause();
        else
            Console.WriteLine("Task is already paused.");
    }

    private void RestartTaskAction(TaskEntity task)
    {
        if (task.Status == TaskStatusEnum.Paused)
            task.Resume();
        else
            Console.WriteLine("Task is not paused.");
    }

    private void FixConsolePosition()
    {
        Console.Clear();
        Console.SetCursorPosition(0, menuHeight);
    }
}
