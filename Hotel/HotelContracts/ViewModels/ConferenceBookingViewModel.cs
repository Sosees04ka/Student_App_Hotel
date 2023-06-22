using HotelDataModels.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelContracts.ViewModels
{
    public class ConferenceBookingViewModel : IConferenceBookingModel
    {
        [DisplayName("Дата начала конференции")]
        public DateTime? BookingDate { get; set; }
        public int HeadwaiterId { get; set; }

        public int? ConferenceId { get; set; }

        public string ConfName { get; set; } = string.Empty;

        public int Id { get; set; }
        public string NameHall { get; set; } = string.Empty;
        public Dictionary<int, IDinnerModel> ConferenceBookingDinners { get; set; } = new();

        public Dictionary<int, IConferenceBookingModel> ConferenceConferenceBookings { get; set; } = new();
        public ConferenceBookingViewModel() { }

        [JsonConstructor]
        public ConferenceBookingViewModel(Dictionary<int, DinnerViewModel> ConferenceBookingDinners, Dictionary<int, ConferenceBookingViewModel> ConferenceConferenceBookings) 
        {
            this.ConferenceBookingDinners = ConferenceBookingDinners.ToDictionary(x => x.Key, x => x.Value as IDinnerModel);
            this.ConferenceConferenceBookings = ConferenceConferenceBookings.ToDictionary(x => x.Key, x => x.Value as IConferenceBookingModel);
        }
    }
}
