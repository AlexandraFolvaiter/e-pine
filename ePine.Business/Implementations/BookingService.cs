using System.Globalization;
using ePine.Business.Contracts;
using ePine.DataAccess.Connections;
using ePine.DataAccess.Entities;
using ePine.DataAccess.Repositories.Contracts;
using ePine.Models;
using Square.Exceptions;
using Square.Models;

namespace ePine.Business.Implementations;

public class BookingService : BaseService, IBookingService
{
    private readonly SquareConnection _squareConnection;
    private readonly IAppointmentRepository _appointmentRepository;

    public BookingService(
        IMerchantRepository merchantRepository,
        IAppointmentRepository appointmentRepository,
        SquareConnection squareConnection)
        : base(merchantRepository)
    {
        _squareConnection = squareConnection;
        _appointmentRepository = appointmentRepository;
    }

    public IList<TeamMemberBookingProfile> GetTeamMembers(Guid merchantId)
    {
        var merchant = MerchantRepository.GetById(merchantId);

        if (merchant == null)
        {
            // TODO: show error
        }

        var client = _squareConnection.GetSquareClient(merchant.AccessToken);


        var teamMembers = client.BookingsApi.RetrieveTeamMemberBookingProfile("TMhWT25VK-W_mso_");

        return new List<TeamMemberBookingProfile>
        {
            teamMembers.TeamMemberBookingProfile
        };
    }

    public IList<Availability> SearchAvailabilities(BookingCreateModel model)
    {
        var merchant = MerchantRepository.GetById(model.MerchantId);

        if (merchant == null)
        {
            // TODO: show error
        }

        var startAtRange = new TimeRange.Builder()
            .StartAt(model.SearchAvailabilityStartDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", DateTimeFormatInfo.InvariantInfo))
            .EndAt(model.SearchAvailabilityEndDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", DateTimeFormatInfo.InvariantInfo))
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

    public void CreateAppointment(Guid userId, BookingCreateModel model)
    {
        var merchant = MerchantRepository.GetById(model.MerchantId);

        if (merchant == null)
        {
            // TODO: show error
        }

        var appointmentSegment = new AppointmentSegment.Builder(teamMemberId: model.TeamMemberId)
            .ServiceVariationId(model.ServiceVariantId)
            .ServiceVariationVersion(long.Parse(model.ServiceVariantVersion))
            .Build();

        var appointmentSegments = new List<AppointmentSegment>();
        appointmentSegments.Add(appointmentSegment);

        // TODO: customerId
        var booking = new Booking.Builder()
            .StartAt(model.SelectedStartAt)
            .LocationId(model.LocationId)
            .CustomerId("VDRFA0THA5681A63CVTVXCAR58")
            .CustomerNote(model.CustomerNote)
            .AppointmentSegments(appointmentSegments)
            .Build();

        var body = new CreateBookingRequest.Builder(booking: booking)
            .Build();

        try
        {
            var client = _squareConnection.GetSquareClient(merchant.AccessToken);
            var result =  client.BookingsApi.CreateBooking(body).Booking;
            var appointment = new Appointment
            {
                UserId = userId,
                MerchantId = merchant.Id,
                AppointmentId = result.Id,
                DateAndTime = DateTime.Parse(result.StartAt),
                Name = "ceva??/"
            };

            _appointmentRepository.AddAppointment(appointment);
        }
        catch (ApiException e)
        {
            Console.WriteLine("Failed to make the request");
            Console.WriteLine($"Response Code: {e.ResponseCode}");
            Console.WriteLine($"Exception: {e.Message}");
        }
    }

    public void CancelAppointment(Guid appointmentId)
    {
        var localAppointment = _appointmentRepository.GetAppointment(appointmentId);

        var merchant = MerchantRepository.GetById(localAppointment.MerchantId);

        var client = _squareConnection.GetSquareClient(merchant?.AccessToken);
        var body = new CancelBookingRequest.Builder()
            .Build();
        var result = client.BookingsApi.CancelBooking(localAppointment.AppointmentId, body).Booking;

        _appointmentRepository.RemoveAppointment(localAppointment);

    }

    public IList<AppointmentModel> GetGeneralAppointments(Guid userId)
    {
        var list = _appointmentRepository.GetAll(userId);

        return list.Select(a => new AppointmentModel
        {
            Id = a.Id,
            MerchantId = a.MerchantId,
            AppointmentId = a.AppointmentId,
            DateAndTime = a.DateAndTime,
            UserId = a.UserId,
            Name = a.Name
        }).ToList();
    }

    public IList<AppointmentModel> GetGeneralAppointmentsByMerchantId(Guid userId, Guid merchantId)
    {
        var list = _appointmentRepository
            .GetAll(userId)
            .Where(a => a.MerchantId == merchantId);

        return list.Select(a => new AppointmentModel
        {
            Id = a.Id,
            MerchantId = a.MerchantId,
            AppointmentId = a.AppointmentId,
            DateAndTime = a.DateAndTime,
            UserId = a.UserId,
            Name = a.Name
        })
            .OrderBy(a => a.DateAndTime)
            .ToList();
    }

    public AppointmentModel GetAppointment(Guid appointmentId)
    {
        throw new NotImplementedException();
    }
}