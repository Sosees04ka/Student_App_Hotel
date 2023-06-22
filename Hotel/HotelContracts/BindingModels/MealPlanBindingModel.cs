using HotelDataModels.Models;

namespace HotelContracts.BindingModels
{
    public class MealPlanBindingModel : IMealPlanModel
    {
        public string MealPlanName { get; set; } = string.Empty;

        public double MealPlanPrice { get; set; }

        public int OrganiserId { get; set; }

        public int Id { get; set; }
        public Dictionary<int, IMemberModel> MealPlanMembers { get; set; } = new();
    }
}
