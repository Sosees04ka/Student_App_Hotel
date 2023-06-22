using HotelContracts.ViewModels;

namespace HotelBusinessLogic.OfficePackage.HelperModels
{
    public class WordInfoOrganiser
    {
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<ReportMemberConferenceViewModel> MemberConferences { get; set; } = new();

    }
}
