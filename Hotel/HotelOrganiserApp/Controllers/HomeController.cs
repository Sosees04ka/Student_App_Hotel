using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;
using HotelOrganiserApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Web.Helpers;
using System.Reflection;
using HotelBusinessLogic.BusinessLogics;
using HotelContracts.BusinessLogicsContracts;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;

namespace HotelOrganiserApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReportOrganiserLogic _report;

        public HomeController(ILogger<HomeController> logger, IReportOrganiserLogic report)
        {
            _logger = logger;
            _report = report;
        }

        /*--------------------Reports------------------------*/

        [HttpGet]
        public IActionResult ListMemberConferenceToFile()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<MemberViewModel>>($"api/main/getmemberlist?organiserId={APIClient.Organiser.Id}"));
        }

        [HttpPost]
        public void ListMemberConferenceToFile(int[] Ids, string type)
        {
            if (APIClient.Organiser == null)
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
                APIClient.PostRequest("api/report/createreporttowordfile", new ReportBindingModel
                {
                    Ids = res,
                    FileName= "C:\\ReportsCourseWork\\wordfile.docx"
                });
                Response.Redirect("GetWordFile");
            }
            else
            {
                APIClient.PostRequest("api/report/createreporttoexcelfile", new ReportBindingModel
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
        public IActionResult GetPdfFile()
        {
            return new PhysicalFileResult("C:\\ReportsCourseWork\\pdffile.pdf", "application/pdf");
        }
        public IActionResult GetExcelFile()
        {
            return new PhysicalFileResult("C:\\ReportsCourseWork\\excelfile.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpGet]
        public IActionResult ListMembersToPdfFile()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View("ListMembersToPdfFile");
        }

        [HttpPost]
        public void ListMembersToPdfFile(DateTime dateFrom, DateTime dateTo, string organiserEmail)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Вы как суда попали? Суда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(organiserEmail))
            {
                throw new Exception("Email пуст");
            }
            APIClient.PostRequest("api/report/CreateReportToPdfFile", new ReportBindingModel
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                OrganiserId = APIClient.Organiser.Id
            });
            APIClient.PostRequest("api/report/SendPdfToMail", new MailSendInfoBindingModel
            {
                MailAddress = organiserEmail,
                Subject = "Отчет по участникам (pdf)",
                Text = "Отчет по участникам с " + dateFrom.ToShortDateString() + " до " + dateTo.ToShortDateString()
            });
            Response.Redirect("ListMembersToPdfFile");
        }

        [HttpGet]
        public string GetMembersReport(DateTime dateFrom, DateTime dateTo)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Вы как суда попали? Суда вход только авторизованным");
            }
            List<ReportMembersViewModel> result;
            try
            {
                result = _report.GetMembers(new ReportBindingModel
                {
                    OrganiserId = APIClient.Organiser.Id,
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
            table += $"<h2 class=\"u-text u-text-custom-color-1 u-text-default u-text-1\">Предварительный отчет</h2>";
            table += $"<table class=\"u-table-entity\">";
            table += "<colgroup>";
            table += "<col width=\"20%\" />";
            table += "<col width=\"20%\" />";
            table += "<col width=\"20%\" />";
            table += "<col width=\"20%\" />";
            table += "<col width=\"20%\" />";
            table += "</colgroup>";
            table += "<thead class=\"u-custom-color-1 u-table-header u-table-header-1\">";
            table += "<tr style=\"height: 31px\">";
            table += $"<th class=\"u-border-1 u-border-grey-50 u-table-cell\">Участник</th>";
            table += $"<th class=\"u-border-1 u-border-grey-50 u-table-cell\">Конференция</th>";
            table += $"<th class=\"u-border-1 u-border-grey-50 u-table-cell\">Дата начала конференции</th>";
            table += $"<th class=\"u-border-1 u-border-grey-50 u-table-cell\">План питания</th>";
            table += $"<th class=\"u-border-1 u-border-grey-50 u-table-cell\">Стоимость плана питания</th>";
            table += "</tr>";
            table += "</thead>";
            foreach (var report in result)
            {
                bool IsDate = true;
                if (report.StartDate.ToShortDateString() == "01.01.0001")
                {
                    IsDate = false;
                }
                bool IsCost = true;
                if (report.MealPlanPrice.ToString() == "0")
                {
                    IsCost = false;
                }
                table += "<tbody class=\"u-table-body\">";
                table += "<tr style=\"height: 75px\">";
                table += $"<td class=\"u-border-1 u-border-grey-40 u-border-no-left u-border-no-right u-table-cell\">{report.MemberFIO}</td>";
                table += $"<td class=\"u-border-1 u-border-grey-40 u-border-no-left u-border-no-right u-table-cell\">{report.ConferenceName}</td>";
                table += $"<td class=\"u-border-1 u-border-grey-40 u-border-no-left u-border-no-right u-table-cell\">{(IsDate is true ? report.StartDate.ToShortDateString() : string.Empty)}</td>";
                table += $"<td class=\"u-border-1 u-border-grey-40 u-border-no-left u-border-no-right u-table-cell\">{report.MealPlanName}</td>";
                table += $"<td class=\"u-border-1 u-border-grey-40 u-border-no-left u-border-no-right u-table-cell\">{(IsCost is true ? report.MealPlanPrice.ToString(): string.Empty)}</td>";
                table += "</tr>";
                table += "</tbody>";
                sum+= report.MealPlanPrice;
            }
            table += "</table>";
            table += $"<h2 class=\"u-text u-text-custom-color-1 u-text-default u-text-1\">Итого: {sum}</h2>";
            return table;
        }
    
        /*--------------------MealPlans------------------------*/

        [HttpGet]
        public IActionResult AddRoomToMealPlan()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.MealPlans = APIClient.GetRequest<List<MealPlanViewModel>>($"api/main/getmealplanlist?organiserId={APIClient.Organiser.Id}");
            ViewBag.Rooms = APIClient.GetRequest<List<RoomViewModel>>($"api/main/getrooms");
            return View();
        }

        [HttpPost]
        public void AddRoomToMealPlan(int mealPlan, int room)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            var roomElem = APIClient.GetRequest<RoomViewModel>($"api/main/getroombyid?roomId={room}");
            APIClient.PostRequest("api/main/updateroom", new RoomBindingModel
            {
                Id = room,
                MealPlanId = mealPlan,
                RoomFrame = roomElem.RoomFrame,
                RoomName = roomElem.RoomName,
                RoomPrice = roomElem.RoomPrice,
                HeadwaiterId = roomElem.HeadwaiterId,
            });
            Response.Redirect("ListMealPlans");
        }

        public IActionResult CreateMealPlan()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void CreateMealPlan(string mealPlanName, double mealPlanPrice)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(mealPlanName) || string.IsNullOrEmpty(mealPlanPrice.ToString()))
            {
                throw new Exception("Введите название");
            }
            if (string.IsNullOrEmpty(mealPlanPrice.ToString()))
            {
                throw new Exception("Введите стоимость");
            }
            APIClient.PostRequest("api/main/createmealplan", new MealPlanBindingModel
            {
                MealPlanName = mealPlanName,
                MealPlanPrice = mealPlanPrice,
                OrganiserId = APIClient.Organiser.Id,
            });
            Response.Redirect("ListMealPlans");
        }

        public IActionResult DeleteMealPlan()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.MealPlans = APIClient.GetRequest<List<MealPlanViewModel>>($"api/main/getmealplanlist?organiserId={APIClient.Organiser.Id}");
            return View();
        }

        [HttpPost]
        public void DeleteMealPlan(int mealPlan)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            APIClient.PostRequest("api/main/deletemealplan", new MealPlanBindingModel
            {
                Id = mealPlan
            });
            Response.Redirect("ListMealPlans");
        }

        public IActionResult UpdateMealPlan()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.MealPlans = APIClient.GetRequest<List<MealPlanViewModel>>($"api/main/getmealplanlist?organiserId={APIClient.Organiser.Id}");
            return View();
        }

        [HttpPost]
        public void UpdateMealPlan(int mealPlan, string mealPlanName, double mealPlanPrice)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(mealPlanName))
            {
                throw new Exception("Название не может быть пустым");
            }
            if (string.IsNullOrEmpty(mealPlanPrice.ToString()))
            {
                throw new Exception("стоимсоть не может быть пустым");
            }
            APIClient.PostRequest("api/main/updatemealplan", new MealPlanBindingModel
            {
                Id = mealPlan,
                MealPlanName = mealPlanName,
                MealPlanPrice = mealPlanPrice,
                OrganiserId = APIClient.Organiser.Id
            });
            Response.Redirect("ListMealPlans");
        }

        [HttpGet]
        public Tuple<MealPlanViewModel, string, string>? GetMealPlan(int mealPlanId)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIClient.GetRequest<Tuple<MealPlanViewModel, List<Tuple<string, string>>, List<Tuple<string, string>>>>($"api/main/getmealplan?mealPlanId={mealPlanId}");
            if (result == null)
            {
                return default;
            }
            string table = "";
            for (int i = 0; i < result.Item2.Count; i++)
            {
                var memberFIO = result.Item2[i].Item1;
                var citizenship = result.Item2[i].Item2;
                table += "<tr style=\"height: 44px\">";
                table += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{memberFIO}</td>";
                table += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{citizenship}</td>";
                table += "</tr>";
            }
            string tablerooms = "";
            for (int i = 0; i < result.Item3.Count; i++)
            {
                var roomName = result.Item3[i].Item1;
                var roomFrame = result.Item3[i].Item2;
                tablerooms += "<tr style=\"height: 44px\">";
                tablerooms += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{roomName}</td>";
                tablerooms += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{roomFrame}</td>";
                tablerooms += "</tr>";
            }
            return Tuple.Create(result.Item1, table, tablerooms);
        }

        public IActionResult AddMemberToMealPlan()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(Tuple.Create(APIClient.GetRequest<List<MealPlanViewModel>>($"api/main/getmealplanlist?organiserId={APIClient.Organiser.Id}"), 
                APIClient.GetRequest<List<MemberViewModel>>($"api/main/getmemberlist?organiserId={APIClient.Organiser.Id}")));
        }

        [HttpPost]
        public void AddMemberToMealPlan(int mealPlan, int[] member)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            for (int i = 0; i < member.Length; i++)
            {
                APIClient.PostRequest("api/main/AddMemberToMealPlan", Tuple.Create(
                new MealPlanSearchModel() { Id = mealPlan },
                new MemberViewModel() { Id = member[i] }
                ));
            }
            Response.Redirect("ListMealPlans");
        }

        public IActionResult ListMealPlans()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<MealPlanViewModel>>($"api/main/getmealplanlist?organiserId={APIClient.Organiser.Id}"));
        }

        /*--------------------Members------------------------*/

        public IActionResult CreateMember()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void CreateMember(string fio, string citizenship)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(fio) || string.IsNullOrEmpty(citizenship))
            {
                throw new Exception("Введите фио");
            }
            if (string.IsNullOrEmpty(citizenship))
            {
                throw new Exception("Введите гражданство");
            }
            APIClient.PostRequest("api/main/createmember", new MemberBindingModel
            {
                MemberFIO = fio,
                Citizenship = citizenship,
                OrganiserId = APIClient.Organiser.Id,
            });
            Response.Redirect("ListMembers");
        }

        public IActionResult UpdateMember()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Members = APIClient.GetRequest<List<MemberViewModel>>($"api/main/getmemberlist?organiserId={APIClient.Organiser.Id}");
            return View();
        }

        [HttpPost]
        public void UpdateMember(int member, string fio, string citizenship)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(fio))
            {
                throw new Exception("фио не может быть пустым");
            }
            if (string.IsNullOrEmpty(citizenship))
            {
                throw new Exception("Гражданство не может быть пустым");
            }

            APIClient.PostRequest("api/main/updatemember", new MemberBindingModel
            {
                Id = member,
                MemberFIO = fio,
                Citizenship = citizenship,
                OrganiserId = APIClient.Organiser.Id,
            });

            Response.Redirect("ListMembers");
        }

        public IActionResult DeleteMember()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Members = APIClient.GetRequest<List<MemberViewModel>>($"api/main/getmemberlist?organiserId={APIClient.Organiser.Id}");
            return View();
        }

        [HttpPost]
        public void DeleteMember(int member)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            APIClient.PostRequest("api/main/deletemember", new MemberBindingModel
            {
                Id = member
            });
            Response.Redirect("ListMembers");
        }

        [HttpGet]
        public MemberViewModel? GetMember(int memberId)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIClient.GetRequest<MemberViewModel>($"api/main/getmember?memberid={memberId}");
            if (result == null)
            {
                return default;
            }
                var memberFIO = result.MemberFIO;
                var citizenship = result.Citizenship;

            return result;
        }

        public IActionResult Index()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        public IActionResult ListMembers()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<MemberViewModel>>($"api/main/getmemberlist?organiserId={APIClient.Organiser.Id}"));
        }

        /*--------------------Conferences------------------------*/

        public IActionResult CreateConference()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void CreateConference(string conferenceName, DateTime startDate)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(conferenceName) || string.IsNullOrEmpty(startDate.ToString()))
            {
                throw new Exception("Введите название");
            }
            if (string.IsNullOrEmpty(startDate.ToString()))
            {
                throw new Exception("Введите дату");
            }
            APIClient.PostRequest("api/main/createconference", new ConferenceBindingModel
            {
                ConferenceName = conferenceName,
                StartDate = startDate,
                OrganiserId = APIClient.Organiser.Id,
            });
            Response.Redirect("ListConferences");
        }

        public IActionResult DeleteConference()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Conferences = APIClient.GetRequest<List<ConferenceViewModel>>($"api/main/getconferencelist?organiserId={APIClient.Organiser.Id}");
            return View();
        }

        [HttpPost]
        public void DeleteConference(int conference)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            APIClient.PostRequest("api/main/deleteconference", new ConferenceBindingModel
            {
                Id = conference
            });
            Response.Redirect("ListConferences");
        }

        public IActionResult UpdateConference()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Conferences = APIClient.GetRequest<List<ConferenceViewModel>>($"api/main/getconferencelist?organiserId={APIClient.Organiser.Id}");
            return View();
        }

        [HttpPost]
        public void UpdateConference(int conference, string conferenceName, DateTime startDate)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            if (string.IsNullOrEmpty(conferenceName))
            {
                throw new Exception("Название не может быть пустым");
            }
            if (string.IsNullOrEmpty(startDate.ToString()))
            {
                throw new Exception("Дата не может быть пустым");
            }
            APIClient.PostRequest("api/main/updateconference", new ConferenceBindingModel
            {
                Id = conference,
                ConferenceName = conferenceName,
                StartDate = startDate,
                OrganiserId = APIClient.Organiser.Id
            });
            Response.Redirect("ListConferences");
        }

        [HttpGet]
        public Tuple<ConferenceViewModel, string>? GetConference(int conferenceId)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            var result = APIClient.GetRequest<Tuple<ConferenceViewModel, List<Tuple<string, string>>>>($"api/main/getconference?conferenceId={conferenceId}");
            if (result == null)
            {
                return default;
            }
            string table = "";
            for (int i = 0; i < result.Item2.Count; i++)
            {
                var memberFIO = result.Item2[i].Item1;
                var citizenship = result.Item2[i].Item2;
                table += "<tr style=\"height: 44px\">";
                table += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{memberFIO}</td>";
                table += $"<td class=\"u-border-1 u-border-grey-30 u-table-cell\">{citizenship}</td>";
                table += "</tr>";
            }
            return Tuple.Create(result.Item1, table);
        }

        public IActionResult AddMemberToConference()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(Tuple.Create(APIClient.GetRequest<List<ConferenceViewModel>>($"api/main/getconferencelist?organiserId={APIClient.Organiser.Id}"),
                APIClient.GetRequest<List<MemberViewModel>>($"api/main/getmemberlist?organiserId={APIClient.Organiser.Id}")));
        }

        [HttpPost]
        public void AddMemberToConference(int conference, int[] member)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Необходима авторизация");
            }
            for (int i = 0; i < member.Length; i++)
            {
                APIClient.PostRequest("api/main/AddMemberToConference", Tuple.Create(
                    new ConferenceSearchModel() { Id = conference },
                    new MemberViewModel() { Id = member[i] }
                ));
            }
            Response.Redirect("ListConferences");
        }

        public IActionResult ListConferences()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<ConferenceViewModel>>($"api/main/getconferencelist?organiserId={APIClient.Organiser.Id}"));
        }

        /*--------------------Organisers------------------------*/

        [HttpGet]
        public IActionResult Privacy()
        {
            if (APIClient.Organiser == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.Organiser);
        }

        [HttpPost]
        public void Privacy(string login, string email, string password, string fio, string telephone)
        {
            if (APIClient.Organiser == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fio))
            {
                throw new Exception("Введите логин, пароль и ФИО");
            }
            APIClient.PostRequest("api/organiser/updatedata", new OrganiserBindingModel
            {
                Id = APIClient.Organiser.Id,
                OrganiserFIO = fio,
                OrganiserLogin = login,
                OrganiserPassword = password,
                OrganiserEmail= email,
                OrganiserNumber= telephone
            });

            APIClient.Organiser.OrganiserFIO = fio;
            APIClient.Organiser.OrganiserLogin = login;
            APIClient.Organiser.OrganiserPassword = password;
            APIClient.Organiser.OrganiserEmail = email;
            APIClient.Organiser.OrganiserNumber = telephone;
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
            APIClient.Organiser = APIClient.GetRequest<OrganiserViewModel>($"api/organiser/login?login={login}&password={password}");
            if (APIClient.Organiser == null)
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
            APIClient.PostRequest("api/organiser/register", new OrganiserBindingModel
            {
                OrganiserFIO = fio,
                OrganiserLogin = login,
                OrganiserPassword = password,
                OrganiserEmail = email,
                OrganiserNumber = telephone
            });

            Response.Redirect("Enter");
            return;
        }

    }
}