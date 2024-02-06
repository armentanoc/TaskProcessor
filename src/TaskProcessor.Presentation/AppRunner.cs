using TaskProcessor.Application.Services;
using TaskProcessor.Domain.Model;
using TaskProcessor.Presentation.Helpers;

public class AppRunner
{
    private readonly SubTaskService _subTaskService;
    private readonly TaskService _taskService;
    private readonly TaskExecutionService _taskExecutionService;
    private CancellationTokenSource _cancellationTokenSource;

    public AppRunner(SubTaskService subTaskService, TaskService taskService, TaskExecutionService taskExecutionService)
    {
        _subTaskService = subTaskService;
        _taskService = taskService;
        _taskExecutionService = taskExecutionService;
    }

    public async Task Run()
    {
        DefaultData.TryInserting(_subTaskService, _taskService);

        try
        {
            int concurrentTasksCount = 2;

            _cancellationTokenSource = new CancellationTokenSource();

            Task displayTask = _taskService.DisplayInformationAboutAllTasksAsync();
            Task executeTask = _taskExecutionService.ExecuteTopTasksWithSubTasksAsync(concurrentTasksCount, _cancellationTokenSource.Token);

            await Task.WhenAll(displayTask, executeTask);

            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message, ex.StackTrace);
        }
        finally
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}
