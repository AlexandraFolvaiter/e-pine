using Square.Models;

namespace ePine.Models;

public class AppointmentDetails
{
    public AppointmentModel AppointmentModel { get; set; }
    public Booking Booking { get; set; }
    public string MerchanName { get; set; }
    public string Location { get; set; }
    public string TeamMember { get; set; }
}