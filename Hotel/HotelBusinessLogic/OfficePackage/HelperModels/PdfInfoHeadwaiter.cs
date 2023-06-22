using HotelContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBusinessLogic.OfficePackage.HelperModels
{
    public class PdfInfoHeadwaiter
    {
        public string FileName { get; set; } = "C:\\ReportsCourseWork\\pdffile.pdf";
        public string Title { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<ReportDinnersViewModel> Dinners { get; set; } = new();
    }
}
