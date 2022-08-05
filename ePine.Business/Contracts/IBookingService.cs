using ePine.Models;
using Square.Models;

namespace ePine.Business.Contracts;

public interface IBookingService
{
    IList<Availability> SearchAvailabilities(BookingCreateModel model);
    Booking CreateAppointment(BookingCreateModel model);
}