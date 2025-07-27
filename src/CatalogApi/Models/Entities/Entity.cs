namespace WebApi.Models.Entities;

public abstract class Entity
{
    protected Entity()
    { }

    public DateTime? CreatedOn { get; set; } = DateTimeOffset.UtcNow.DateTime;
    public DateTime? UpdatedOn { get; set; } = DateTimeOffset.UtcNow.DateTime;
}
