namespace TaskProcessor.Domain.Model
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        public Customer(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Customer ID: {Id}, Name: {Name}";
        }
    }
}
