namespace ePine.Models;

public class AppointmentModel
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid MerchantId { get; set; }

    public string AppointmentId { get; set; }

    public string Name { get; set; }

    public DateTime DateAndTime { get; set; }

    public bool IsInThePast => DateAndTime <= DateTime.Now;
}