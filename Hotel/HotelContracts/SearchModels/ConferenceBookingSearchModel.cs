
namespace HotelContracts.SearchModels
{
   public class ConferenceBookingSearchModel
    {
        public int? Id { get; set; }
        public int? HeadwaiterId { get; set; }
        public int? ConferenceId { get; set; }
        public DateTime? BookingDate { get; set; }
        public string? NameHall { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
