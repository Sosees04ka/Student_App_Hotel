namespace HotelContracts.SearchModels
{
    public class ConferenceSearchModel
    {
        public int? Id { get; set; }
        public string? ConferenceName { get; set; }
        public int? OrganiserId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
