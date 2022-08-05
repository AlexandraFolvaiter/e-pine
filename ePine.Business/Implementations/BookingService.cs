using System.Globalization;
using ePine.Business.Contracts;
using ePine.DataAccess.Connections;
using ePine.Models;
using Square.Models;

namespace ePine.Business.Implementations;

public class BookingService : IBookingService
{
    private readonly SquareConnection _squareConnection;

    public BookingService(SquareConnection squareConnection)
    {
        _squareConnection = squareConnection;
    }
    public IList<Availability> SearchAvailabilities(BookingCreateModel model)
    {
        var startAtRange = new TimeRange.Builder()
            .StartAt(model.SearchAvailabilityStartDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK'Z'", DateTimeFormatInfo.InvariantInfo))
            .EndAt(model.SearchAvailabilityEndDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK'Z'", DateTimeFormatInfo.InvariantInfo))
            .Build();

        var segmentFilter = new SegmentFilter
                .Builder(serviceVariationId: model.ServiceVariantId)
            .Build();

        var segmentFilters = new List<SegmentFilter>();
        segmentFilters.Add(segmentFilter);

        var filter = new SearchAvailabilityFilter.Builder(startAtRange: startAtRange)
            // TODO: change location
            .LocationId("LTBAH8P3MGT9B")
            .SegmentFilters(segmentFilters)
            .Build();

        var query = new SearchAvailabilityQuery.Builder(filter: filter)
            .Build();

        var body = new SearchAvailabilityRequest.Builder(query: query)
            .Build();

        // this will be taken from database
        // TODO: add merchants into database
        var accessToken = "EAAAED7dgvUge5TXjM_LFraCeUeM_CNzYkuYIS8c1CPC0F7KpVF3XQgc0_NageqY";

        var client = _squareConnection.GetSquareClient(accessToken);

        var availabilities = client.BookingsApi.SearchAvailability(body).Availabilities;

        return availabilities;
    }
}