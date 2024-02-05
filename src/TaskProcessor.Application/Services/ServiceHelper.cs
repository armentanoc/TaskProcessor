﻿
using TaskProcessor.Domain.Interfaces;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Application.Services
{
    public class ServiceHelper
    {
        internal static TaskStatusEnum[] GetExcludedStatuses()
        {
            return new[]
            {
                TaskStatusEnum.Completed,
                TaskStatusEnum.Cancelled,
                TaskStatusEnum.Paused
            };
        }

        internal static bool IsCompleted<T>(T entity) where T : IStatusCheckable
        {
            return entity != null && entity.Status == TaskStatusEnum.Completed;
        }
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
        public static void LogWithColor(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Log(message);
            Console.ResetColor();
        }
        public static void LogProgress(TaskEntity task)
        {
            var completed = task.CompletedSubTasks;
            var total = task.TotalSubTasks;
            LogWithColor(ConsoleColor.Blue, $"\nProgress: {completed}/{total} subtasks completed for Task {task.Id}");
            LogProgressBar(completed, total);
        }
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

        internal static void LogError(Exception ex)
        {
            LogWithColor(ConsoleColor.Red, $"Erro: {ex.Message} - {ex.StackTrace}");
        }
    }
}
