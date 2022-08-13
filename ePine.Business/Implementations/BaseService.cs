using ePine.DataAccess.Repositories.Contracts;

namespace ePine.Business.Implementations;

public class BaseService
{
    public BaseService(IMerchantRepository merchantRepository)
    {
        MerchantRepository = merchantRepository;
    }

    protected IMerchantRepository MerchantRepository { get; set; }
}