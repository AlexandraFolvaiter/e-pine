using Square.Models;

namespace ePine.Business.Contracts;

public interface ILocationService
{
    IList<Location> GetLocations(Guid merchantId);
}