namespace HotelDataModels.Models
{
    public interface IConferenceBookingModel : IId
    {
        int HeadwaiterId { get;}
        int? ConferenceId { get; }
        DateTime? BookingDate { get; }
        public Dictionary<int, IDinnerModel> ConferenceBookingDinners { get; }
    }
}
