using HotelContracts.BindingModels;
using HotelContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IReportHeadwaiterLogic
    {
        List<ReportDinnerRoomViewModel> GetDinnerRoom(List<int> Ids);
        List<ReportDinnersViewModel> GetDinners(ReportHeadwaiterBindingModel model);
        void SaveDinnerRoomToWordFile(ReportHeadwaiterBindingModel model);
        void SaveDinnerRoomToExcelFile(ReportHeadwaiterBindingModel model);
        void SaveDinnersToPdfFile(ReportHeadwaiterBindingModel model);
    }
}
