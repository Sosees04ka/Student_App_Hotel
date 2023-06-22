namespace HotelDataModels.Models
{
    public interface IMealPlanModel : IId
    {
        string MealPlanName { get; }
        double MealPlanPrice { get; }
        int OrganiserId { get; }
        public Dictionary<int, IMemberModel> MealPlanMembers { get; }
    }
}
