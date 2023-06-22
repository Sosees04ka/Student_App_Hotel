namespace HotelContracts.ViewModels
{
    public class ReportMembersViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public string MemberFIO { get; set; } = string.Empty;
        public string ConferenceName { get; set; } = string.Empty;
        public string MealPlanName { get; set; } = string.Empty;
        public double MealPlanPrice { get; set; }
    }
}
