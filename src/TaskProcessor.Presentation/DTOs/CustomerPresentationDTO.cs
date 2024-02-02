namespace TaskProcessor.Presentation.DTOs
{
    public record CustomerPresentationDTO
    {
        public string Name { get; init; }

        public CustomerPresentationDTO(string name)
        {
            Name = name;
        }
    }
}
