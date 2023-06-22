
using HotelDataModels.Models;
using System.ComponentModel;
using Newtonsoft.Json;

namespace HotelContracts.ViewModels
{
    public class RoomViewModel : IRoomModel
    {
        public int HeadwaiterId { get; set; }

        public int? MealPlanId { get; set; }

        public int Id { get; set; }

        [DisplayName("Название комнаты")]
        public string RoomName { get; set; } = string.Empty;

        [DisplayName("Корпус комнаты")]
        public string RoomFrame { get; set; } = string.Empty;

        [DisplayName("Стоимость комнаты")]
        public double RoomPrice { get; set; }
        public Dictionary<int, IDinnerModel> RoomDinners { get; set; } = new();

        public RoomViewModel() { }

        [JsonConstructor]
        public RoomViewModel(Dictionary<int, DinnerViewModel> RoomDinners)
        {
            this.RoomDinners = RoomDinners.ToDictionary(x => x.Key, x => x.Value as IDinnerModel);
        }
    }
}
