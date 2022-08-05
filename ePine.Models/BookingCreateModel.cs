namespace ePine.Models;

public class BookingCreateModel
{
    public Guid MerchantId { get; set; }

    public string? ServiceVariantId { get; set; }
    public string? SelectedStartAt { get; set; }

    public DateTime SearchAvailabilityStartDate { get; set; }
    public DateTime SearchAvailabilityEndDate { get; set; }

    public BookingCreateModel()
    {
        SearchAvailabilityStartDate = DateTime.Now;
        SearchAvailabilityEndDate = DateTime.Now.AddDays(3);
    }
}