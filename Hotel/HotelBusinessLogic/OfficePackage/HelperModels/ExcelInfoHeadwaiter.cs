using HotelContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBusinessLogic.OfficePackage.HelperModels
{
    public class ExcelInfoHeadwaiter
    {
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<ReportDinnerRoomViewModel> DinnerRooms
        {
            get;
            set;
        } = new();
    }
}
