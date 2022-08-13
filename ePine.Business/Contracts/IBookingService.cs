using ePine.Models;
using Square.Models;

namespace ePine.Business.Contracts;

public interface IBookingService
{
    IList<TeamMember> GetTeamMembers(Guid merchantId);
    IList<Availability> SearchAvailabilities(BookingCreateModel model);
    void CreateAppointment(Guid userId, BookingCreateModel model);
    void CancelAppointment(Guid appointmentId);
    IList<AppointmentModel> GetGeneralAppointments(Guid userId);
    IList<AppointmentModel> GetGeneralAppointmentsByMerchantId(Guid userId, Guid merchantId);
    AppointmentDetails GetBooking(Guid appointmentId);
}