
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Application.Services;

namespace TaskProcessor.Presentation.Helpers
{
    internal class DefaultData
    {
        internal static void TryInserting(int numberOfTasks, ISubTaskService subTaskService, ITaskService taskService)
        {
            try
            {
                if (!subTaskService.GetAllSubTasks().Any() && !taskService.GetAllTasks().Any())
                {
                    InsertDefaultTasks(numberOfTasks, subTaskService, taskService);
                    Console.WriteLine("Default data inserted.");
                }
                else
                {
                    Console.WriteLine("TaskEntity and SubTaskEntity already exist. No changes made.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting: " + ex.Message, ex.StackTrace);
            } 
        }

        private static void InsertDefaultTasks(int numberOfTasks, ISubTaskService subTaskService, ITaskService taskService)
        {
            for (int i = 0; i < numberOfTasks; i++) 
            {
                taskService.CreateTask();
            }
        }
    }
}