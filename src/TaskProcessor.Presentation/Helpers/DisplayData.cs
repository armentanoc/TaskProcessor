using TaskProcessor.Domain.Model;
using TaskProcessor.Presentation.Helpers;

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

    internal static async Task DisplayProgress(Func<Task<IEnumerable<TaskEntity>>> value)
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
}
