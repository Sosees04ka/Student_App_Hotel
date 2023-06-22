using HotelDataModels.Models;

namespace HotelContracts.BindingModels
{
    public class ConferenceBookingBindingModel : IConferenceBookingModel
    {
        public int HeadwaiterId { get; set; }

        public int? ConferenceId { get; set; }

        public string NameHall { get; set; } = string.Empty;

        public DateTime? BookingDate { get; set; }

        public int Id { get; set; }
        public Dictionary<int, IDinnerModel> ConferenceBookingDinners { get; set; } = new();
    }
}
