using System.Globalization;
using ePine.Business.Contracts;
using ePine.DataAccess.Connections;
using ePine.DataAccess.Entities;
using ePine.DataAccess.Repositories.Contracts;
using ePine.Models;
using Microsoft.AspNetCore.Identity;
using Square;
using Square.Models;

namespace ePine.Business.Implementations;

public class BookingService : BaseService, IBookingService
{
    private readonly SquareConnection _squareConnection;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public BookingService(
        IMerchantRepository merchantRepository,
        IAppointmentRepository appointmentRepository,
        SquareConnection squareConnection,
        UserManager<IdentityUser> userManager)
        : base(merchantRepository)
    {
        _squareConnection = squareConnection;
        _userManager = userManager;
        _appointmentRepository = appointmentRepository;
    }

    public IList<TeamMember> GetTeamMembers(Guid merchantId)
    {
        var merchant = MerchantRepository.GetById(merchantId);

        var client = _squareConnection.GetSquareClient(merchant?.AccessToken);

        var teamMembers = client.TeamApi.SearchTeamMembers(new SearchTeamMembersRequest()).TeamMembers;
        return teamMembers;
    }

    public IList<Availability> SearchAvailabilities(BookingCreateModel model)
    {
        var merchant = MerchantRepository.GetById(model.MerchantId);

        var startAtRange = new TimeRange.Builder()
            .StartAt(model.SearchAvailabilityStartDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", DateTimeFormatInfo.InvariantInfo))
            .EndAt(model.SearchAvailabilityEndDate.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", DateTimeFormatInfo.InvariantInfo))
            .Build();

        var segmentFilter = new SegmentFilter
                .Builder(model.ServiceVariantId)
            .Build();

        var segmentFilters = new List<SegmentFilter> { segmentFilter };

        var filter = new SearchAvailabilityFilter.Builder(startAtRange)
            .LocationId(model.LocationId)
            .SegmentFilters(segmentFilters)
            .Build();

        var query = new SearchAvailabilityQuery.Builder(filter)
            .Build();

        var body = new SearchAvailabilityRequest.Builder(query)
            .Build();

        var client = _squareConnection.GetSquareClient(merchant?.AccessToken);

        var availabilities = client.BookingsApi.SearchAvailability(body).Availabilities;

        return availabilities;
    }

    public void CreateAppointment(Guid userId, BookingCreateModel model)
    {
        var merchant = MerchantRepository.GetById(model.MerchantId);
        var client = _squareConnection.GetSquareClient(merchant.AccessToken);
        var appointmentSegment = new AppointmentSegment.Builder(model.TeamMemberId)
            .ServiceVariationId(model.ServiceVariantId)
            .ServiceVariationVersion(long.Parse(model.ServiceVariantVersion))
            .Build();

        var appointmentSegments = new List<AppointmentSegment> { appointmentSegment };

        var booking = new Booking.Builder()
            .StartAt(model.SelectedStartAt)
            .LocationId(model.LocationId)
            .CustomerId(GetCustomerId(userId, client))
            .CustomerNote(model.CustomerNote)
            .AppointmentSegments(appointmentSegments)
            .Build();

        var body = new CreateBookingRequest.Builder( booking)
            .Build();

            var result =  client.BookingsApi.CreateBooking(body).Booking;
            var appointment = new Appointment
            {
                UserId = userId,
                MerchantId = merchant.Id,
                AppointmentId = result.Id,
                DateAndTime = DateTime.Parse(result.StartAt),
                Name = model.ServiceName
            };

        _appointmentRepository.AddAppointment(appointment);
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
        })
            .OrderBy(a => a.DateAndTime)
            .ToList();
    }

    public IList<AppointmentModel> GetGeneralAppointmentsByMerchantId(Guid userId, Guid merchantId)
    {
        var list = _appointmentRepository
            .GetAll(userId)
            .Where(a => a.DateAndTime > DateTime.Now)
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

    public AppointmentDetails GetBooking(Guid appointmentId)
    {
        var localAppointment = _appointmentRepository.GetAppointment(appointmentId);

        var merchant = MerchantRepository.GetById(localAppointment.MerchantId);

        var client = _squareConnection.GetSquareClient(merchant?.AccessToken);
        var booking = client
            .BookingsApi
            .RetrieveBooking(localAppointment.AppointmentId)
            .Booking;

        var location = client.LocationsApi.RetrieveLocation(booking.LocationId).Location;

        var teamMember = client
            .BookingsApi
            .RetrieveTeamMemberBookingProfile(booking.
                AppointmentSegments
                .FirstOrDefault()?
                .TeamMemberId)
            .TeamMemberBookingProfile;

        var appointmentDetails = new AppointmentDetails
        {
            Booking = booking,
            AppointmentModel = new AppointmentModel
            {
                Id = localAppointment.Id,
                MerchantId = localAppointment.MerchantId,
                AppointmentId = localAppointment.AppointmentId,
                DateAndTime = localAppointment.DateAndTime,
                UserId = localAppointment.UserId,
                Name = localAppointment.Name
            },
            MerchanName = merchant.Name,
            Location = $"{location.Name} - {location.Address.AddressLine1}, {location.Address.Locality}, {location.Address.Country}, {location.Address.PostalCode}",
            TeamMember = teamMember.DisplayName
        };
        return appointmentDetails;
    }

    private string? GetCustomerId(Guid userId, SquareClient? client)
    {
        var user = _userManager.FindByIdAsync(userId.ToString()).Result;

        var emailAddress = new CustomerTextFilter.Builder()
            .Exact(user.Email)
            .Build();

        var filter = new CustomerFilter.Builder()
            .EmailAddress(emailAddress)
            .Build();

        var query = new CustomerQuery.Builder()
            .Filter(filter)
            .Build();

        var body = new SearchCustomersRequest.Builder()
            .Query(query)
            .Build();

        var result = client.CustomersApi.SearchCustomers(body).Customers;

        if (result!= null && result.Any())
        {
            return result.FirstOrDefault()?.Id;
        }

        var createBody = new CreateCustomerRequest.Builder()
            .GivenName(user.UserName)
            .EmailAddress(user.Email)
            .Build();
        var createResult = client.CustomersApi.CreateCustomer(createBody).Customer;

        return createResult.Id;
    }
}