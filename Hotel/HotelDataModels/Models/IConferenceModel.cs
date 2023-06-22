namespace HotelDataModels.Models
{
    public interface IConferenceModel : IId
    {
        string ConferenceName { get; }
        DateTime StartDate { get; }
        int OrganiserId { get; }
        public Dictionary<int, IMemberModel> ConferenceMembers { get; }
    }
}
