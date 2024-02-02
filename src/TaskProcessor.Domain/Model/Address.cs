namespace TaskProcessor.Domain.Model
{
    public class Address : BaseEntity
    {
        public string Street { get; set; }
        public uint Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public override string ToString()
        {
            return $"Address ID: {Id}, " +
                    $"Street: {Street}, " +
                    $"Number: {Number}, " +
                    (string.IsNullOrEmpty(Complement) ? "" : $"Complement: {Complement}") +
                    $"City: {City}, " +
                    $"State: {State}, " +
                    $"ZipCode: {ZipCode}";
        }
    }
}
