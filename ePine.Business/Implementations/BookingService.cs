using System.Globalization;
using ePine.Business.Contracts;
using ePine.DataAccess.Connections;
using ePine.DataAccess.Repositories.Contracts;
using ePine.Models;
using Square.Exceptions;
using Square.Models;

namespace ePine.Business.Implementations;

public class BookingService : BaseService, IBookingService
{
    private readonly SquareConnection _squareConnection;

    public BookingService(IMerchantRepository merchantRepository, SquareConnection squareConnection)
        : base(merchantRepository)
    {
        _squareConnection = squareConnection;
    }

    public IList<Availability> SearchAvailabilities(BookingCreateModel model)
    {
        var merchant = MerchantRepository.GetById(model.MerchantId);

        if (merchant == null)
        {
            // TODO: show error
        }

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

        var client = _squareConnection.GetSquareClient(merchant.AccessToken);

        var availabilities = client.BookingsApi.SearchAvailability(body).Availabilities;

        return availabilities;
    }

    public Booking CreateAppointment(BookingCreateModel model)
    {
        var merchant = MerchantRepository.GetById(model.MerchantId);

        if (merchant == null)
        {
            // TODO: show error
        }

        var appointmentSegment = new AppointmentSegment.Builder(teamMemberId: "TMhWT25VK-W_mso_")
            .ServiceVariationId(model.ServiceVariantId)
            .ServiceVariationVersion(long.Parse(model.ServiceVariantVersion))
            .Build();

        var appointmentSegments = new List<AppointmentSegment>();
        appointmentSegments.Add(appointmentSegment);

        // TODO: replace locationID
        // TODO: customerId
        var booking = new Booking.Builder()
            .StartAt(model.SelectedStartAt)
            .LocationId("LTBAH8P3MGT9B")
            .CustomerId("VDRFA0THA5681A63CVTVXCAR58")
            .CustomerNote(model.CustomerNote)
            .AppointmentSegments(appointmentSegments)
            // .LocationType("BUSINESS_LOCATION")
            .Build();

        var body = new CreateBookingRequest.Builder(booking: booking)
            .Build();

        try
        {
            var client = _squareConnection.GetSquareClient(merchant.AccessToken);
            var result =  client.BookingsApi.CreateBooking(body).Booking;
            return result;
        }
        catch (ApiException e)
        {
            Console.WriteLine("Failed to make the request");
            Console.WriteLine($"Response Code: {e.ResponseCode}");
            Console.WriteLine($"Exception: {e.Message}");
        }

        return null;
    }
}