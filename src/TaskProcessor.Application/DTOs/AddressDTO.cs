namespace TaskProcessor.Application.DTOs;
public record AddressDTO
{
    public int Id { get; init; }
    public string Street { get; init; }
    public uint Number { get; init; }
    public string? Complement { get; init; }
    public string Neighborhood { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string ZipCode { get; init; }

    public AddressDTO(int id, string street, uint number, string? complement, string neighborhood, string city, string state, string zipCode)
    {
        Id = id;
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
}
