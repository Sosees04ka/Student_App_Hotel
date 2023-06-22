using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelContracts.ViewModels
{
    public class ReportDinnerRoomViewModel
    {
        public string DinnerName { get; set; } = string.Empty;
        public List<Tuple<string, double>> Rooms { get; set; } = new();

    }
}
