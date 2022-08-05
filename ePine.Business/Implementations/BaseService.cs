using ePine.DataAccess.Repositories.Contracts;
using ePine.DataAccess.Repositories.Implementations;

namespace ePine.Business.Implementations;

public class BaseService
{
    public BaseService(IMerchantRepository merchantRepository)
    {
        MerchantRepository = merchantRepository;
    }

    protected IMerchantRepository MerchantRepository { get; set; }
}