namespace RapidAPIBuilder.Models.Entties;

public interface IEntity
{
    Guid Id { get; set; }
}

public interface IDeletable
{
    bool IsDeleted { get; set; }
}

public class Tag : IEntity, IDeletable
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsDeleted { get; set; }
}
