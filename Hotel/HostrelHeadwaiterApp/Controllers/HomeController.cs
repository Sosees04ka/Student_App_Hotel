using HostrelHeadwaiterApp.Models;
using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HostrelHeadwaiterApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReportHeadwaiterLogic _report;

        public HomeController(ILogger<HomeController> logger, IReportHeadwaiterLogic report)
        {
            _logger = logger;
            _report = report;
        }

        public IActionResult CreateDinner()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void CreateDinner(double dinnerPrice, string dinnerName)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(dinnerName))
            {
                throw new Exception("Введите имя");
            }
            APIClient.PostRequest("api/main/createdinner", new DinnerBindingModel
            {
                DinnerPrice = dinnerPrice,
                DinnerName = dinnerName,
                HeadwaiterId = APIClient.Headwaiter.Id,
            });
            Response.Redirect("ListDinners");
        }

        public IActionResult UpdateDinner()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Dinners = APIClient.GetRequest<List<DinnerViewModel>>($"api/main/getdinnerlist?headwaiterId={APIClient.Headwaiter.Id}");
            return View();
        }

        [HttpPost]
        public void UpdateDinner(int dinner, string dinnerName, double dinnerPrice)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(dinnerName))
            {
                throw new Exception("имя не может быть пустым");
            }

            APIClient.PostRequest("api/main/updatedinner", new DinnerBindingModel
            {
                Id = dinner,
                DinnerName = dinnerName,
                DinnerPrice = dinnerPrice,
                HeadwaiterId = APIClient.Headwaiter.Id,
            });

            Response.Redirect("ListDinners");
        }

        public IActionResult DeleteDinner()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Dinners = APIClient.GetRequest<List<DinnerViewModel>>($"api/main/getdinnerlist?headwaiterId={APIClient.Headwaiter.Id}");
            return View();
        }

        [HttpPost]
        public void DeleteDinner(int dinner)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            APIClient.PostRequest("api/main/deletedinner", new DinnerBindingModel
            {
                Id = dinner
            });
            Response.Redirect("ListDinners");
        }

        [HttpGet]
        public DinnerViewModel? GetDinner(int dinnerId)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIClient.GetRequest<DinnerViewModel>($"api/main/getdinner?dinnerid={dinnerId}");
            if (result == null)
            {
                return default;
            }
            var dinnerPrice = result.DinnerPrice;
            var dinnerName = result.DinnerName;

            return result;
        }

        public IActionResult Index()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        public IActionResult ListDinners()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<DinnerViewModel>>($"api/main/getdinnerlist?headwaiterId={APIClient.Headwaiter.Id}"));
        }

        public IActionResult CreateRoom()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void CreateRoom(string roomName, double roomPrice, string roomFrame)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(roomName))
            {
                throw new Exception("Введите название");
            }
            if (string.IsNullOrEmpty(roomPrice.ToString()))
            {
                throw new Exception("Введите цену");
            }
            APIClient.PostRequest("api/main/createroom", new RoomBindingModel
            {
                RoomName = roomName,
                RoomPrice = roomPrice,
                RoomFrame = roomFrame,
                HeadwaiterId = APIClient.Headwaiter.Id,
            });
            Response.Redirect("ListRooms");
        }

        public IActionResult DeleteRoom()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Rooms = APIClient.GetRequest<List<RoomViewModel>>($"api/main/getroomlist?headwaiterId={APIClient.Headwaiter.Id}");
            return View();
        }

        [HttpPost]
        public void DeleteRoom(int room)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            APIClient.PostRequest("api/main/deleteroom", new RoomBindingModel
            {
                Id = room
            });
            Response.Redirect("ListRooms");
        }

        public IActionResult UpdateRoom()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Rooms = APIClient.GetRequest<List<RoomViewModel>>($"api/main/getroomlist?headwaiterId={APIClient.Headwaiter.Id}");
            return View();
        }

        [HttpPost]
        public void UpdateRoom(int room, string roomName, double roomPrice, string roomFrame)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(roomName))
            {
                throw new Exception("Введите название");
            }
            if (string.IsNullOrEmpty(roomPrice.ToString()))
            {
                throw new Exception("Введите цену");
            }
            APIClient.PostRequest("api/main/updateroom", new RoomBindingModel
            {
                Id = room,
                RoomName = roomName,
                RoomPrice = roomPrice,
                RoomFrame = roomFrame,
                HeadwaiterId = APIClient.Headwaiter.Id
            });
            Response.Redirect("ListRooms");
        }

        [HttpGet]
        public Tuple<RoomViewModel, string>? GetRoom(int roomId)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIClient.GetRequest<Tuple<RoomViewModel, List<Tuple<string, string>>>>($"api/main/getroom?roomId={roomId}");
            if (result == null)
            {
                return default;
            }
            string table = "";
            for (int i = 0; i < result.Item2.Count; i++)
            {
                var dinnerName = result.Item2[i].Item1;
                var dinnerPrice = result.Item2[i].Item2;
                table += "<tr style=\"height: 44px\">";
                table += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{dinnerName}</td>";
                table += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{dinnerPrice}</td>";
                table += "</tr>";
            }
            return Tuple.Create(result.Item1, table);
        }

        public IActionResult AddDinnerToRoom()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(Tuple.Create(APIClient.GetRequest<List<RoomViewModel>>($"api/main/getroomlist?headwaiterId={APIClient.Headwaiter.Id}"),
            APIClient.GetRequest<List<DinnerViewModel>>($"api/main/getdinnerlist?headwaiterId={APIClient.Headwaiter.Id}")));
        }

        [HttpPost]
        public void AddDinnerToRoom(int room, int[] dinner)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            for (int i = 0; i < dinner.Length; i++)
            {
                    APIClient.PostRequest("api/main/AddDinnerToRoom", Tuple.Create(
                    new RoomSearchModel() { Id = room },
                    new DinnerViewModel() { Id = dinner[i] }
                ));
            }
            Response.Redirect("ListRooms");
        }

        public IActionResult ListRooms()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<RoomViewModel>>($"api/main/getroomlist?headwaiterId={APIClient.Headwaiter.Id}"));
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.Headwaiter);
        }

        [HttpPost]
        public void Privacy(string login, string email, string password, string fio, string telephone)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fio))
            {
                throw new Exception("Введите логин, пароль и ФИО");
            }
            APIClient.PostRequest("api/headwaiter/updatedata", new HeadwaiterBindingModel
            {
                Id = APIClient.Headwaiter.Id,
                HeadwaiterFIO = fio,
                HeadwaiterLogin = login,
                HeadwaiterPassword = password,
                HeadwaiterEmail = email,
                HeadwaiterNumber = telephone
            });

            APIClient.Headwaiter.HeadwaiterFIO = fio;
            APIClient.Headwaiter.HeadwaiterLogin = login;
            APIClient.Headwaiter.HeadwaiterPassword = password;
            APIClient.Headwaiter.HeadwaiterEmail = email;
            APIClient.Headwaiter.HeadwaiterNumber = telephone;
            Response.Redirect("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public void Enter(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Введите логин и пароль");
            }
            APIClient.Headwaiter = APIClient.GetRequest<HeadwaiterViewModel>($"api/headwaiter/login?login={login}&password={password}");
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Неверный логин/пароль");
            }
            Response.Redirect("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(string login, string email, string password, string fio, string telephone)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fio))
            {
                throw new Exception("Введите логин, пароль и ФИО");
            }
            APIClient.PostRequest("api/headwaiter/register", new HeadwaiterBindingModel
            {
                HeadwaiterFIO = fio,
                HeadwaiterLogin = login,
                HeadwaiterPassword = password,
                HeadwaiterEmail = email,
                HeadwaiterNumber = telephone
            });

            Response.Redirect("Enter");
            return;
        }

        public IActionResult CreateConferenceBooking()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void CreateConferenceBooking(string nameHall)
        {
            if (string.IsNullOrEmpty(nameHall))
            {
                throw new Exception("Введите название");
            }
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
   
            APIClient.PostRequest("api/main/createconferenceBooking", new ConferenceBookingBindingModel
            {
                NameHall = nameHall,
                HeadwaiterId = APIClient.Headwaiter.Id,
            });
            Response.Redirect("ListConferenceBookings");
        }

        public IActionResult DeleteConferenceBooking()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.ConferenceBookings = APIClient.GetRequest<List<ConferenceBookingViewModel>>($"api/main/getconferenceBookinglist?headwaiterId={APIClient.Headwaiter.Id}");
            return View();
        }

        [HttpPost]
        public void DeleteConferenceBooking(int conferenceBooking)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            APIClient.PostRequest("api/main/deleteconferenceBooking", new ConferenceBookingBindingModel
            {
                Id = conferenceBooking
            });
            Response.Redirect("ListConferenceBookings");
        }

        public IActionResult UpdateConferenceBooking()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.ConferenceBookings = APIClient.GetRequest<List<ConferenceBookingViewModel>>($"api/main/getconferenceBookinglist?headwaiterId={APIClient.Headwaiter.Id}");
            ViewBag.Conferences = APIClient.GetRequest<List<ConferenceViewModel>>($"api/main/getconferences");
            return View();
        }

        [HttpPost]
        public void UpdateConferenceBooking(int conference, int conferenceBooking, string nameHall, DateTime bookingDate)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(nameHall))
            {
                throw new Exception("Название не может быть пустым");
            }
            APIClient.PostRequest("api/main/updateconferenceBooking", new ConferenceBookingBindingModel
            {
                Id = conferenceBooking,
                NameHall = nameHall,
                BookingDate = bookingDate,
                HeadwaiterId = APIClient.Headwaiter.Id,
                ConferenceId = conference
            });
            Response.Redirect("ListConferenceBookings");
        }

        [HttpGet]
        public Tuple<ConferenceBookingViewModel, string>? GetConferenceBooking(int conferenceBookingId)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIClient.GetRequest<Tuple<ConferenceBookingViewModel, List<Tuple<string, string>>>>($"api/main/getconferenceBooking?conferenceBookingId={conferenceBookingId}");
            if (result == null)
            {
                return default;
            }
            string table = "";
            for (int i = 0; i < result.Item2.Count; i++)
            {
                var dinnerName = result.Item2[i].Item1;
                var dinnerPrice = result.Item2[i].Item2;
                table += "<tr style=\"height: 44px\">";
                table += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{dinnerName}</td>";
                table += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{dinnerPrice}</td>";
                table += "</tr>";
            }
            return Tuple.Create(result.Item1, table);
        }

        public IActionResult AddDinnerToConferenceBooking()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(Tuple.Create(APIClient.GetRequest<List<ConferenceBookingViewModel>>($"api/main/getconferencebookinglist?headwaiterId={APIClient.Headwaiter.Id}"),
              APIClient.GetRequest<List<DinnerViewModel>>($"api/main/getdinnerlist?headwaiterId={APIClient.Headwaiter.Id}"))); 
        }

        [HttpPost]
        public void AddDinnerToConferenceBooking(int conferenceBooking, int[] dinner)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Необходима авторизация");
            }
            for (int i = 0; i < dinner.Length; i++)
            {
                APIClient.PostRequest("api/main/AddDinnerToConferenceBooking", Tuple.Create(
                new ConferenceBookingSearchModel() { Id = conferenceBooking },
                new DinnerViewModel() { Id = dinner[i] }
            ));
            }
            Response.Redirect("ListConferenceBookings");
        }
            

        public IActionResult ListConferenceBookings()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<ConferenceBookingViewModel>>($"api/main/getconferenceBookinglist?headwaiterId={APIClient.Headwaiter.Id}"));
        }

        [HttpGet]
        public IActionResult AddConferenceBookingToConference()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Conferences = APIClient.GetRequest<List<ConferenceViewModel>>($"api/main/getconferences");
            ViewBag.ConferenceBookings = APIClient.GetRequest<List<ConferenceBookingViewModel>>($"api/main/getconferenceBookinglist?headwaiterId={APIClient.Headwaiter.Id}");
            return View();
        }

        [HttpPost]
        public void AddConferenceBookingToConference(int conference, int conferenceBooking, DateTime bookingDate)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            var conferenceBookingElem = APIClient.GetRequest<ConferenceBookingViewModel>($"api/main/getconferenceBookingbyid?conferenceBookingId={conferenceBooking}");
            APIClient.PostRequest("api/main/updateconferenceBooking", new ConferenceBookingBindingModel
            {
                Id = conferenceBooking,
                ConferenceId = conference,
                NameHall = conferenceBookingElem.NameHall,
                BookingDate = bookingDate,
                HeadwaiterId = conferenceBookingElem.HeadwaiterId,
            });
            Response.Redirect("ListConferenceBookings");
        }

        /*--------------------Reports------------------------*/

        [HttpGet]
        public IActionResult ListDinnerRoomToFile()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<DinnerViewModel>>($"api/main/getdinnerlist?headwaiterId={APIClient.Headwaiter.Id}"));
        }

        [HttpPost]
        public void ListDinnerRoomToFile(int[] Ids, string type)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Вы как суда попали? Суда вход только авторизованным");
            }

            if (Ids.Length <= 0)
            {
                throw new Exception("Количество должно быть больше 0");
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("Неверный тип отчета");
            }

            List<int> res = new List<int>();

            foreach (var item in Ids)
            {
                res.Add(item);
            }

            if (type == "docx")
            {
                APIClient.PostRequest("api/report/createheadwaiterreporttowordfile", new ReportHeadwaiterBindingModel
                {
                    Ids = res,
                    FileName = "C:\\ReportsCourseWork\\wordfile.docx"
                });
                Response.Redirect("GetWordFile");
            }
            else
            {
                APIClient.PostRequest("api/report/createheadwaiterreporttoexcelfile", new ReportHeadwaiterBindingModel
                {
                    Ids = res,
                    FileName = "C:\\ReportsCourseWork\\excelfile.xlsx"
                });
                Response.Redirect("GetExcelFile");
            }
        }

        [HttpGet]
        public IActionResult GetWordFile()
        {
            return new PhysicalFileResult("C:\\ReportsCourseWork\\wordfile.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

        public IActionResult GetExcelFile()
        {
            return new PhysicalFileResult("C:\\ReportsCourseWork\\excelfile.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public IActionResult GetPdfFile()
        {
            return new PhysicalFileResult("C:\\ReportsCourseWork\\pdffile.pdf", "application/pdf");
        }

        [HttpGet]
        public IActionResult ListDinnersToPdfFile()
        {
            if (APIClient.Headwaiter == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View("ListDinnersToPdfFile");
        }

        [HttpPost]
        public void ListDinnersToPdfFile(DateTime dateFrom, DateTime dateTo, string headwaiterEmail)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Вы как суда попали? Суда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(headwaiterEmail))
            {
                throw new Exception("Email пуст");
            }
            APIClient.PostRequest("api/report/CreateHeadwaiterReportToPdfFile", new ReportHeadwaiterBindingModel
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                HeadwaiterId = APIClient.Headwaiter.Id
            });
            APIClient.PostRequest("api/report/SendPdfToMail", new MailSendInfoBindingModel
            {
                MailAddress = headwaiterEmail,
                Subject = "Отчет по обедам (pdf)",
                Text = "Отчет по обедам с " + dateFrom.ToShortDateString() + " до " + dateTo.ToShortDateString()
            });
            Response.Redirect("ListDinnersToPdfFile");
        }

        [HttpGet]
        public string GetDinnersReport(DateTime dateFrom, DateTime dateTo)
        {
            if (APIClient.Headwaiter == null)
            {
                throw new Exception("Вы как суда попали? Суда вход только авторизованным");
            }
            List<ReportDinnersViewModel> result;
            try
            {
                result = _report.GetDinners(new ReportHeadwaiterBindingModel
                {
                    HeadwaiterId = APIClient.Headwaiter.Id,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания отчета");
                throw;
            }
            double sum = 0;
            string table = "";
            table += "<h2 class=\"text-custom-color-1\">Предварительный отчет</h2>";
            table += "<div class=\"table-responsive\">";
            table += "<table class=\"table table-striped table-bordered table-hover\">";
            table += "<thead class=\"table-dark\">";
            table += "<tr>";
            table += "<th scope=\"col\">Обед</th>";
            table += "<th scope=\"col\">Имя комнаты</th>";
            table += "<th scope=\"col\">Цена комнаты</th>";
            table += "<th scope=\"col\">Название зала</th>";
            table += "<th scope=\"col\">Дата броинирования</th>";
            table += "</tr>";
            table += "</thead>";
            foreach (var report in result)
            {
                bool IsCost = true;
                if (report.RoomPrice == 0)
                {
                    IsCost = false;
                }
                table += "<tbody>";
                table += "<tr>";
                table += $"<td>{report.DinnerName}</td>";
                table += $"<td>{report.RoomName}</td>";
                table += $"<td>{(IsCost ? report.RoomPrice.ToString() : string.Empty)}</td>";
                table += $"<td>{report.NameHall}</td>";
                table += $"<td>{report.BookingDate?.ToShortDateString()}</td>";
                table += "</tr>";
                table += "</tbody>";
                sum += report.RoomPrice;
            }
            table += "<tfoot class=\"table-secondary\">";
            table += $"<tr><th colspan=\"2\">Итого:</th><th>{sum}</th><th colspan=\"2\"></th></tr>";
            table += "</tfoot>";
            table += "</table>";
            table += "</div>";
            return table;
        }
    }
}