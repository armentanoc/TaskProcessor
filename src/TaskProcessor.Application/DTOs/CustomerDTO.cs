namespace TaskProcessor.Application.DTOs;
public record CustomerDTO
{
    public int Id { get; init; }
    public string Name { get; init; }

    public CustomerDTO(int id, string? name)
    {
        Id = id;
        Name = name;
    }
}