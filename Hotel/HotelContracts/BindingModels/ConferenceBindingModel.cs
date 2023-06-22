using HotelDataModels.Models;

namespace HotelContracts.BindingModels
{
    public class ConferenceBindingModel : IConferenceModel
    {
        public string ConferenceName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; } = DateTime.Now;

        public int Id { get; set; }

        public int OrganiserId { get; set; }
        public Dictionary<int, IMemberModel> ConferenceMembers { get; set; } = new();
    }
}
