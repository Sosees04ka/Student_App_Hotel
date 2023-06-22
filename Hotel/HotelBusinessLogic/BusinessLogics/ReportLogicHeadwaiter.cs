using HotelBusinessLogic.OfficePackage.HelperModels;
using HotelBusinessLogic.OfficePackage;
using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataBaseImplement;
using HotelDataBaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelContracts.BusinessLogicsContracts;

namespace HotelBusinessLogic.BusinessLogics
{
    public class ReportLogicHeadwaiter : IReportHeadwaiterLogic
    {
        private readonly IRoomStorage _roomStorage;
        private readonly IDinnerStorage _dinnerStorage;
        private readonly IConferenceBookingStorage _conferenceBookingStorage;
        private readonly AbstractSaveToExcelHeadwaiter _saveToExcel;
        private readonly AbstractSaveToWordHeadwaitre _saveToWord;
        private readonly AbstractSaveToPdfHeadwaiter _saveToPdf;
        public ReportLogicHeadwaiter(IRoomStorage roomStorage, IDinnerStorage dinnerStorage, IConferenceBookingStorage conferenceBookingStorage, AbstractSaveToExcelHeadwaiter saveToExcel, AbstractSaveToWordHeadwaitre saveToWord, AbstractSaveToPdfHeadwaiter saveToPdf)
        {
            _roomStorage = roomStorage;
            _dinnerStorage = dinnerStorage;
            _conferenceBookingStorage = conferenceBookingStorage;
            _saveToExcel = saveToExcel;
            _saveToWord = saveToWord;
            _saveToPdf = saveToPdf;
        }
        public List<ReportDinnerRoomViewModel> GetDinnerRoom(List<int> Ids)
        {
            if (Ids == null)
            {
                return new List<ReportDinnerRoomViewModel>();
            }
            var rooms = _roomStorage.GetFullList();
            List<DinnerViewModel> dinners = new List<DinnerViewModel>();
            foreach (var memId in Ids)
            {
                var res = _dinnerStorage.GetElement(new DinnerSearchModel { Id = memId });
                if (res != null)
                {
                    dinners.Add(res);
                }
            }
            var list = new List<ReportDinnerRoomViewModel>();
            foreach (var dinner in dinners)
            {
                var record = new ReportDinnerRoomViewModel
                {
                    DinnerName = dinner.DinnerName,
                    Rooms = new List<Tuple<string, double>>()
                };
                foreach (var room in rooms)
                {
                    if (room.RoomDinners.ContainsKey(dinner.Id))
                    {
                        record.Rooms.Add(new Tuple<string, double>(room.RoomName, room.RoomPrice));
                    }
                }
                list.Add(record);
            }
            return list;
        }

        public List<ReportDinnersViewModel> GetDinners(ReportHeadwaiterBindingModel model)
        {
            var listAll = new List<ReportDinnersViewModel>();

            var listRooms = _roomStorage.GetFilteredList(new RoomSearchModel
            {
                HeadwaiterId = model.HeadwaiterId,

            });

            foreach (var room in listRooms)
            {
                foreach (var m in room.RoomDinners.Values)
                {
                    listAll.Add(new ReportDinnersViewModel
                    {
                        RoomName = room.RoomName,
                        RoomPrice = room.RoomPrice,
                        DinnerName = m.DinnerName,
                        DinnerPrice = m.DinnerPrice
                    });
                }
            }
            var listConferenceBookings = _conferenceBookingStorage.GetFilteredList(new ConferenceBookingSearchModel
            {
                HeadwaiterId = model.HeadwaiterId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });

            foreach (var conferenceBooking in listConferenceBookings)
            {
                foreach (var mp in conferenceBooking.ConferenceBookingDinners.Values)
                {
                    listAll.Add(new ReportDinnersViewModel
                    {
                        DinnerName = mp.DinnerName,
                        DinnerPrice = mp.DinnerPrice,
                        NameHall = conferenceBooking.NameHall,
                        BookingDate = conferenceBooking.BookingDate

                    });
                }
            }

            return listAll;
        }

        public void SaveDinnerRoomToExcelFile(ReportHeadwaiterBindingModel model)
        {
            _saveToExcel.CreateReport(new ExcelInfoHeadwaiter
            {
                FileName = model.FileName,
                Title = "Список номеров",
                DinnerRooms = GetDinnerRoom(model.Ids)
            });
        }

        public void SaveDinnerRoomToWordFile(ReportHeadwaiterBindingModel model)
        {
            _saveToWord.CreateDoc(new WordInfoHeadwaiter
            {
                FileName = model.FileName,
                Title = "Список номеров",
                DinnerRooms = GetDinnerRoom(model.Ids)
            });
        }

        public void SaveDinnersToPdfFile(ReportHeadwaiterBindingModel model)
        {
            if (model.DateFrom == null)
            {
                throw new ArgumentException("Дата начала не задана");
            }

            if (model.DateTo == null)
            {
                throw new ArgumentException("Дата окончания не задана");
            }

            _saveToPdf.CreateDoc(new PdfInfoHeadwaiter
            {
                FileName = model.FileName,
                Title = "Список обедов",
                DateFrom = model.DateFrom!.Value,
                DateTo = model.DateTo!.Value,
                Dinners = GetDinners(model)
            });
        }
    }
}
