namespace TaskProcessor.Presentation.DTOs
{
    public record AddressPresentationDTO
    {
        public string Street { get; init; }
        public uint Number { get; init; }
        public string? Complement { get; init; }
        public string Neighborhood { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string ZipCode { get; init; }

        public AddressPresentationDTO(string street, uint number, string? complement, string neighborhood, string city, string state, string zipcode)
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipcode;
        }
    }
}
