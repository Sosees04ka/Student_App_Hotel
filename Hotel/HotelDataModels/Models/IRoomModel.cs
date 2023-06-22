
namespace HotelDataModels.Models
{
    public interface IRoomModel : IId
    {
        string RoomName { get; }
        string RoomFrame { get; }
        double RoomPrice { get; }
        int HeadwaiterId { get; }
        int? MealPlanId { get; }
        public Dictionary<int, IDinnerModel> RoomDinners { get; }
    }
}
