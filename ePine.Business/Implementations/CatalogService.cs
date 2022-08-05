using ePine.Business.Contracts;
using ePine.DataAccess.Connections;
using ePine.DataAccess.Repositories.Contracts;
using Square.Models;

namespace ePine.Business.Implementations;

public class CatalogService : BaseService, ICatalogService
{
    private readonly SquareConnection _squareConnection;

    public CatalogService(IMerchantRepository merchantRepository, SquareConnection squareConnection)
        : base(merchantRepository)
    {
        _squareConnection = squareConnection;
    }

    public IList<CatalogObject> GetCatalogItems(Guid merchantId)
    {
        var merchant = MerchantRepository.GetById(merchantId);
        if (merchant == null)
        {
            return new List<CatalogObject>();
        }

        var client = _squareConnection.GetSquareClient(merchant.AccessToken);

        var catalogItems = client.CatalogApi.ListCatalog().Objects;

        return catalogItems;
    }


}