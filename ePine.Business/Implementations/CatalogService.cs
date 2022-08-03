using ePine.Business.Contracts;
using ePine.DataAcess.Connections;
using Square.Models;

namespace ePine.Business.Implementations;

public class CatalogService : ICatalogService
{
    private readonly SquareConnection _squareConnection;

    public CatalogService(SquareConnection squareConnection)
    {
        _squareConnection = squareConnection;
    }

    public IList<CatalogObject> GetCatalogItems(Guid merchantId)
    {
        // this will be taken from database
        // TODO: add merchants into database
        var accessToken = "EAAAED7dgvUge5TXjM_LFraCeUeM_CNzYkuYIS8c1CPC0F7KpVF3XQgc0_NageqY";

        var client = _squareConnection.GetSquareClient(accessToken);
        var catalogItems = client.CatalogApi.ListCatalog().Objects;

        return catalogItems;
    }
}