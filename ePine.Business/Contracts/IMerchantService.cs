using ePine.Models;

namespace ePine.Business.Contracts;

public interface IMerchantService
{
    // TODO: search and filters
    IEnumerable<MerchantModel> GetMerchants();

    MerchantModel GetMerchant(Guid id);
}