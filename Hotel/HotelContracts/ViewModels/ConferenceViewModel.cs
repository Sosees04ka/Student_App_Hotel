using HotelDataModels.Models;
using System.ComponentModel;
using Newtonsoft.Json;

namespace HotelContracts.ViewModels
{
    public class ConferenceViewModel : IConferenceModel
    {
        [DisplayName("Название конференции")]
        public string ConferenceName { get; set; } = string.Empty;

        [DisplayName("Дата начала конференции")]
        public DateTime StartDate { get; set; }

        public int OrganiserId { get; set; }

        public int Id { get; set; }
        public Dictionary<int, IMemberModel> ConferenceMembers { get; set; } = new();
        public Dictionary<int, IConferenceBookingModel> ConferenceConferenceBooking { get; set; }

        public ConferenceViewModel() { }

        [JsonConstructor]
        public ConferenceViewModel(Dictionary<int, MemberViewModel> ConferenceMembers)
        {
            this.ConferenceMembers = ConferenceMembers.ToDictionary(x => x.Key, x => x.Value as IMemberModel);
        }
    }
}
