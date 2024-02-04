namespace TaskProcessor.Application.Interfaces
{
    internal interface ITaskManager
    {
        Task Start(int idTask);
        Task Pause(int idTask);
        Task Stop(int idTask);
        Task Resume(int idTask);
        Task Cancel(int idTask);
    }
}
