namespace HotelContracts.SearchModels
{
    public class MealPlanSearchModel
    {
        public int? Id { get; set; }
        public string? MealPlanName { get; set; }
        public int? OrganiserId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
