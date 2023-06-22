namespace HotelContracts.ViewModels
{
    public class ReportMemberConferenceViewModel
    {
        public string MemberFIO { get; set; } = string.Empty;
        public List<Tuple<string, DateTime>> Conferences { get; set; } = new();
    }
}
