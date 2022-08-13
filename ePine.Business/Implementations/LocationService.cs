using ePine.Business.Contracts;
using ePine.DataAccess.Connections;
using ePine.DataAccess.Repositories.Contracts;
using Square.Models;

namespace ePine.Business.Implementations;

public class LocationService : BaseService, ILocationService
{
    private readonly SquareConnection _squareConnection;

    public LocationService(IMerchantRepository merchantRepository, SquareConnection squareConnection) : base(merchantRepository)
    {
        _squareConnection = squareConnection;
    }

    public IList<Location> GetLocations(Guid merchantId)
    {
        var merchant = MerchantRepository.GetById(merchantId);
        if (merchant == null)
        {
            return new List<Location>();
        }

        var client = _squareConnection.GetSquareClient(merchant.AccessToken);

        var locations = client.LocationsApi.ListLocations().Locations;

        return locations;
    }


}