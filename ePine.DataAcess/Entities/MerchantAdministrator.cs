namespace ePine.DataAccess.Entities;

public class MerchantAdministrator : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid MerchantId { get; set; }
    public virtual Merchant Merchant { get; set; }
}