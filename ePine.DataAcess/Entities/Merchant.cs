namespace ePine.DataAccess.Entities;

public class Merchant : BaseEntity
{
    public Guid AdminId { get; private set; }

    public string? Name { get; private set; }

    public string? Description { get; private set; }

    public string? AccessToken { get; private set; }

    public bool IsPublic { get; private set; }

    public Merchant(string? name, string? description, string? accessToken, Guid adminId, bool isPublic = false)
    {
        Name = name;
        Description = description;
        AccessToken = accessToken;
        AdminId = adminId;
        IsPublic = isPublic;
    }
}