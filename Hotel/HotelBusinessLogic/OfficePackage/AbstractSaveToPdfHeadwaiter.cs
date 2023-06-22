using HotelBusinessLogic.OfficePackage.HelperEnums;
using HotelBusinessLogic.OfficePackage.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToPdfHeadwaiter
    {
        public void CreateDoc(PdfInfoHeadwaiter info)
        {
            CreatePdf(info);
            CreateParagraph(new PdfParagraph
            {
                Text = info.Title,
                Style = "NormalTitle",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });
            CreateParagraph(new PdfParagraph
            {
                Text = $"с {info.DateFrom.ToShortDateString()} по {info.DateTo.ToShortDateString()}",
                Style = "Normal",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });
            CreateTable(new List<string> { "3cm", "3cm", "3cm", "4cm", "4cm" });
            CreateRow(new PdfRowParameters
            {
                Texts = new List<string> { "Обед", "Комната", "Цена комнаты", "Бронирование", "Дата брони" },
                Style = "NormalTitle",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });
            foreach (var dinner in info.Dinners)
            {

                bool IsCost = true;
                if (dinner.RoomPrice.ToString() == "0")
                {
                    IsCost = false;
                }
                CreateRow(new PdfRowParameters
                {
                    Texts = new List<string> { dinner.DinnerName.ToString(), dinner.RoomName, IsCost is true ? dinner.RoomPrice.ToString() : string.Empty, dinner.NameHall, dinner.BookingDate?.ToShortDateString() ?? string.Empty },
                    Style = "Normal",
                    ParagraphAlignment = PdfParagraphAlignmentType.Left
                });
            }
            CreateParagraph(new PdfParagraph
            {
                Text = $"Итого: {info.Dinners.Sum(x => x.RoomPrice)}\t",
                Style = "Normal",
                ParagraphAlignment = PdfParagraphAlignmentType.Rigth
            });
            SavePdf(info);
        }
        protected abstract void CreatePdf(PdfInfoHeadwaiter info);
        protected abstract void CreateParagraph(PdfParagraph paragraph);
        protected abstract void CreateTable(List<string> columns);
        protected abstract void CreateRow(PdfRowParameters rowParameters);
        protected abstract void SavePdf(PdfInfoHeadwaiter info);
    }
}
