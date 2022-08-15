using Square.Models;

namespace ePine.Business.Contracts;

public interface ICatalogService
{
    public IList<CatalogObject> GetCatalogItems(Guid merchantId);
}