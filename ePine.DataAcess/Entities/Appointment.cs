namespace ePine.DataAccess.Entities;

public class Appointment : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid MerchantId { get; set; }
    public string AppointmentId { get; set; }
    public string Name { get; set; }
    public DateTime DateAndTime { get; set; }
}