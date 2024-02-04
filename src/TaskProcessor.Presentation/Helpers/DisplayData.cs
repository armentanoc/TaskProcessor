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
    }
}
