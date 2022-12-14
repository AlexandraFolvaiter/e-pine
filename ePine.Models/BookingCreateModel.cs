namespace ePine.Models;

public class BookingCreateModel
{
    public Guid MerchantId { get; set; }

    public string? ServiceDetails { get; set; }

    public string? ServiceVariantId => ServiceDetails?.Split("&&")[0];

    public string? ServiceVariantVersion => ServiceDetails?.Split("&&")[1];

    public string? ServiceName => ServiceDetails?.Split("&&")[2];

    public string? SelectedStartAt { get; set; }

    public string? LocationId { get; set; }

    public string? TeamMemberId { get; set; }

    public string? CustomerNote { get; set; }

    public DateTime SearchAvailabilityStartDate { get; set; }

    public DateTime SearchAvailabilityEndDate { get; set; }

    public BookingCreateModel()
    {
        SearchAvailabilityStartDate = DateTime.Today;
        SearchAvailabilityEndDate = DateTime.Today.AddDays(3);
    }
}