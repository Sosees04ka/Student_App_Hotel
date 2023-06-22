using HotelContracts.ViewModels;

namespace HotelBusinessLogic.OfficePackage.HelperModels
{
    public class PdfInfoOrganiser
    {
        public string FileName { get; set; } = "C:\\ReportsCourseWork\\pdffile.pdf";
        public string Title { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<ReportMembersViewModel> Members { get; set; } = new();
    }
}
