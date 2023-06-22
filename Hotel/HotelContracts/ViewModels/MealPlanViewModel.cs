using HotelDataModels.Models;
using System.ComponentModel;
using Newtonsoft.Json;

namespace HotelContracts.ViewModels
{
    public class MealPlanViewModel : IMealPlanModel
    {
        [DisplayName("Название плана питания")]
        public string MealPlanName { get; set; } = string.Empty;

        [DisplayName("Цена плана питания")]
        public double MealPlanPrice { get; set; }

        public int OrganiserId { get; set; }

        public int Id { get; set; }

        public Dictionary<int, IMemberModel> MealPlanMembers { get; set; } = new();
        public Dictionary<int, IRoomModel> MealPlanRooms { get; set; } = new();

        public MealPlanViewModel() { }

        [JsonConstructor]
        public MealPlanViewModel(Dictionary<int, MemberViewModel> MealPlanMembers, Dictionary<int, RoomViewModel> MealPlanRooms)
        {
            this.MealPlanMembers = MealPlanMembers.ToDictionary(x => x.Key, x => x.Value as IMemberModel);
            this.MealPlanRooms = MealPlanRooms.ToDictionary(x => x.Key, x => x.Value as IRoomModel);
        }
    }
}
