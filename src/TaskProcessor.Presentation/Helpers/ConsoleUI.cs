using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Presentation.Helpers
{
    internal class ConsoleUI
    {
        private static int menuHeight;

        public static async Task DisplayAsync(int totalTasks, Func<Task<IEnumerable<TaskEntity>>> value, TaskService taskService)
        {
            menuHeight = totalTasks * 3;
            var progressTask = DisplayProgress(value);
            var menuTask = DisplayMenu(taskService, value);

            await Task.WhenAll(progressTask, menuTask);
        }

        private static async Task DisplayProgress(Func<Task<IEnumerable<TaskEntity>>> value)
        {
            while (true)
            {
                var entities = await value.Invoke();
                Console.SetCursorPosition(0, 0);
                foreach (var entity in entities)
                {
                    ConsoleHelper.LogProgress(entity);
                }

                await Task.Delay(1000).ConfigureAwait(false);
            }
        }

        private static async Task DisplayMenu(TaskService taskService, Func<Task<IEnumerable<TaskEntity>>> value)
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
                        await HandlePauseTaskOption(taskService, value);
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
        private static async Task HandlePauseTaskOption(TaskService taskService, Func<Task<IEnumerable<TaskEntity>>> value)
        {
            Console.Clear();
            Console.SetCursorPosition(0, menuHeight);
            Console.WriteLine("\nEnter the ID of the task you want to pause:");

            int inputRow = Console.CursorTop;

            if (int.TryParse(Console.ReadLine(), out int taskToPauseId))
            {
                var taskToPause = taskService.GetTaskById(taskToPauseId);
                if (taskToPause != null)
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
        }
    }

}
