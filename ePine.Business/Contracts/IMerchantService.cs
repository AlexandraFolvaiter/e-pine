using ePine.Models;

namespace ePine.Business.Contracts;

public interface IMerchantService
{
    IEnumerable<MerchantModel> GetMerchants();

    IEnumerable<MerchantModel> GetMerchantsByUser(Guid userId);

    MerchantModel GetMerchant(Guid id);

    void AddMerchant(Guid userId, MerchantModel model);
}