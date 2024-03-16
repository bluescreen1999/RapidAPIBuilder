namespace RapidAPIBuilder.Models.Dtos.Tags;

public record GetTagDetailsResponse
{
    public string Title { get; set; }
    public bool IsDeleted { get; set; }
}
