namespace ePine.DataAccess.Entities;

public class BaseEntity
{
    public Guid Id { get; private set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}