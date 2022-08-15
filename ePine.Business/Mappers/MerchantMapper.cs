using ePine.DataAccess.Entities;
using ePine.Models;

namespace ePine.Business.Mappers;

public static class MerchantMapper
{
    public static Merchant ToEntity(this MerchantModel merchant, Guid userId)
    {
        return new Merchant(merchant.Name, merchant.Description, merchant.AccessToken, userId, true);
    }

    public static MerchantModel ToModel(this Merchant merchant)
    {
        return new MerchantModel
        {
            Id = merchant.Id,
            Name = merchant.Name,
            Description = merchant.Description,
            AccessToken = merchant.AccessToken,
        };
    }
}