namespace ePine.DataAccess.Entities;

public class Merchant : BaseEntity
{

    public string? Name { get; private set; }
    
    public string? Description { get; private set; }

    public string? AccessToken { get; private set; }

    public bool IsPublic { get; private set; }

    public Merchant()
    { }

    public Merchant(string? name, string? description, string? accessToken, bool isPublic = false)
    {
        Name = name;
        Description = description;
        AccessToken = accessToken;
        IsPublic = isPublic;
    }
}