using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelContracts.ViewModels
{
    public class ReportDinnersViewModel
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string NameHall { get; set; } = string.Empty;
        public DateTime? BookingDate { get; set; }
        public string DinnerName { get; set; } = string.Empty;
        public double DinnerPrice { get; set; }
        public double RoomPrice { get; set; }
        public string RoomFrame { get; set; } = string.Empty;

    }
}
