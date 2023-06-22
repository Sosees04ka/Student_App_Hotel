using HotelDataModels.Models;

namespace HotelContracts.BindingModels
{
    public class RoomBindingModel : IRoomModel
    {
        public string RoomName { get; set; } = string.Empty;

        public string RoomFrame { get; set; } = string.Empty;

        public double RoomPrice { get; set; }

        public int HeadwaiterId { get; set; }

        public int? MealPlanId { get; set; }

        public int Id { get; set; }
        public Dictionary<int, IDinnerModel> RoomDinners { get; set; } = new();
    }
}
