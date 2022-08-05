namespace ePine.Models;

public class BookingCreateModel
{
    public Guid MerchantId { get; set; }

    public string? ServiceDetails { get; set; }
    public string? ServiceVariantId => ServiceDetails?.Split("-").First();
    public string? ServiceVariantVersion => ServiceDetails?.Split("-").Last();
    public string? SelectedStartAt { get; set; }

    public string? LocationId { get; set; }
    public string? CustomerNote { get; set; }

    public DateTime SearchAvailabilityStartDate { get; set; }
    public DateTime SearchAvailabilityEndDate { get; set; }

    public BookingCreateModel()
    {
        SearchAvailabilityStartDate = DateTime.Now;
        SearchAvailabilityEndDate = DateTime.Now.AddDays(3);
    }
}