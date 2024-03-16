namespace RapidAPIBuilder.Models.Dtos.Tags;

public record GetAllTagsResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsDeleted { get; set; }
}
