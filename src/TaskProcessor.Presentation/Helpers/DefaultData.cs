using TaskProcessor.Presentation.DTOs;
using TaskProcessor.Application.Services;

namespace TaskProcessor.Presentation.Helpers
{
    internal class DefaultData
    {
        internal static void TryInserting(SubTaskService subTaskService, TaskService taskService)
        {
            try
            {
                if (!subTaskService.GetAllSubTasks().Any() && !taskService.GetAllTasks().Any())
                    InsertDefaultData(subTaskService, taskService);
                else
                    Console.WriteLine("TaskEntity and SubTaskEntity already exist. No changes made.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting: " + ex.Message, ex.StackTrace);
            }
        }

        private static void InsertDefaultData(SubTaskService subTaskService, TaskService taskService)
        {
            InsertDefaultTasks(subTaskService, taskService);
        }

        private static void InsertDefaultTasks(SubTaskService subTaskService, TaskService taskService)
        {
            for (int i = 0; i < 5; i++) //transformar em configuração
            {
                taskService.CreateTask(subTaskService);
            }
        }
    }
}