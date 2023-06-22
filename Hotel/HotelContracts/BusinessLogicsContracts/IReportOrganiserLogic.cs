using HotelContracts.BindingModels;
using HotelContracts.ViewModels;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IReportOrganiserLogic
    {
        List<ReportMemberConferenceViewModel> GetMemberConference(List<int> Ids);
        List<ReportMembersViewModel> GetMembers(ReportBindingModel model);
        void SaveMemberConferenceToWordFile(ReportBindingModel model);
        void SaveMemberConferenceToExcelFile(ReportBindingModel model);
        void SaveMembersToPdfFile(ReportBindingModel model);
    }
}
