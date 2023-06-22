
namespace HotelContracts.SearchModels
{
    public class RoomSearchModel
    {
        public string? RoomName { get; set; }
        public int? HeadwaiterId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? MealPlanId { get; set; }
        public int? Id { get; set; }
    }
}
