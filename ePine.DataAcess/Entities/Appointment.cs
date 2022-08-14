namespace ePine.DataAccess.Entities;

public class Appointment : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid MerchantId { get; private set; }

    public string AppointmentId { get; private set; }

    public string Name { get; private set; }

    public DateTime DateAndTime { get; private set; }

    public Appointment(Guid userId, Guid merchantId, string appointmentId, string name, DateTime dateAndTime)
    {
        UserId = userId;
        MerchantId = merchantId;
        AppointmentId = appointmentId;
        Name = name;
        DateAndTime = dateAndTime;
    }
}