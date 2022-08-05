namespace ePine.Models;

public class MerchantModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? AccessToken { get; set; }

    public bool IsPublic { get; set; }

    // TODO
    // type of business
    // image(s)
}