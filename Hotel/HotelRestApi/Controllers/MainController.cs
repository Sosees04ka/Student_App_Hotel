using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;
using HotelDataBaseImplement;
using HotelDataBaseImplement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO.Packaging;

namespace HotelRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConferenceLogic _conference;
        private readonly IMemberLogic _member;
        private readonly IMealPlanLogic _mealPlan;
        private readonly IDinnerLogic _dinner;
        private readonly IConferenceBookingLogic _conferenceBooking;
        private readonly IRoomLogic _room;

        public MainController(ILogger<MainController> logger, IConferenceLogic conference, IMemberLogic member, IMealPlanLogic mealPlan, IDinnerLogic dinner, IConferenceBookingLogic conferenceBooking, IRoomLogic room)
        {
            _logger = logger;
            _conference = conference;
            _member = member;
            _mealPlan = mealPlan;
            _dinner = dinner;
            _conferenceBooking = conferenceBooking;
            _room = room;
        }

        [HttpGet]
        public List<ConferenceViewModel>? GetConferenceList(int organiserId)
        {
            try
            {
                return _conference.ReadList(new ConferenceSearchModel
                {
                    OrganiserId = organiserId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка конференций");
                throw;
            }
        }

        [HttpGet]
        public List<MemberViewModel>? GetMemberList(int organiserId)
        {
            try
            {
                return _member.ReadList(new MemberSearchModel
                {
                    OrganiserId= organiserId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка участников");
                throw;
            }
        }

        [HttpGet]
        public List<MealPlanViewModel>? GetMealPlanList(int organiserId)
        {
            try
            {
                return _mealPlan.ReadList(new MealPlanSearchModel
                {
                    OrganiserId=organiserId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка планов питания");
                throw;
            }
        }

        [HttpGet]
        public List<DinnerViewModel>? GetDinnerList(int headwaiterId)
        {
            try
            {
                return _dinner.ReadList(new DinnerSearchModel
                {
                    HeadwaiterId = headwaiterId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка обедов");
                throw;
            }
        }

        [HttpGet]
        public List<ConferenceBookingViewModel>? GetConferenceBookingList(int headwaiterId)
        {
            try
            {
                return _conferenceBooking.ReadList(new ConferenceBookingSearchModel
                {
                    HeadwaiterId = headwaiterId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка бронирования по конференциям");
                throw;
            }
        }

        [HttpGet]
        public List<RoomViewModel>? GetRooms()
        {
            try
            {
                return _room.ReadList(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка номеров");
                throw;
            }
        }

        [HttpGet]
        public List<RoomViewModel>? GetRoomList(int headwaiterId)
        {
            try
            {
                return _room.ReadList(new RoomSearchModel
                {
                    HeadwaiterId = headwaiterId,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка номеров");
                throw;
            }
        }

        [HttpPost]
        public void UpdateMember(MemberBindingModel model)
        {
            try
            {
                _member.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }

        [HttpGet]
        public MemberViewModel? GetMember(int memberId)
        {
            try
            {
                return _member.ReadElement(new MemberSearchModel
                {
                    Id = memberId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения участника по id={Id}", memberId);
                throw;
            }
        }

        [HttpPost]
        public void CreateMember(MemberBindingModel model)
        {
            try
            {
                _member.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания участника");
                throw;
            }
        }

        [HttpPost]
        public void DeleteMember(MemberBindingModel model)
        {
            try
            {
                _member.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления участника");
                throw;
            }
        }

        [HttpPost]
        public void UpdateDinner(DinnerBindingModel model)
        {
            try
            {
                _dinner.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных обеда");
                throw;
            }
        }

        [HttpGet]
        public DinnerViewModel? GetDinner(int dinnerId)
        {
            try
            {
                return _dinner.ReadElement(new DinnerSearchModel
                {
                    Id = dinnerId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения обеда по id={Id}", dinnerId);
                throw;
            }
        }

        [HttpPost]
        public void CreateDinner(DinnerBindingModel model)
        {
            try
            {
                _dinner.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания обеда");
                throw;
            }
        }

        [HttpPost]
        public void DeleteDinner(DinnerBindingModel model)
        {
            try
            {
                _dinner.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления обеда");
                throw;
            }
        }

        [HttpPost]
        public void CreateConference(ConferenceBindingModel model)
        {
            try
            {
                _conference.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания конференции");
                throw;
            }
        }

        [HttpPost]
        public void UpdateConference(ConferenceBindingModel model)
        {
            try
            {
                model.ConferenceMembers = null!;
                _conference.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }

        [HttpGet]
        public Tuple<ConferenceViewModel, List<Tuple<string, string>>>? GetConference(int conferenceId)
        {
            try
            {
                var elem = _conference.ReadElement(new ConferenceSearchModel { Id = conferenceId });
                if (elem == null)
                    return null;
                return Tuple.Create(elem, elem.ConferenceMembers.Select(x => Tuple.Create(x.Value.MemberFIO, x.Value.Citizenship)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения конференции по id={Id}", conferenceId);
                throw;
            }
        }

        [HttpPost]
        public void DeleteConference(ConferenceBindingModel model)
        {
            try
            {
                _conference.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления конференции");
                throw;
            }
        }

        [HttpPost]
        public void AddMemberToConference(Tuple<ConferenceSearchModel, MemberViewModel> model)
        {
            try
            {
                _conference.AddMemberToConference(model.Item1, model.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка добавления участника в конференцию.");
                throw;
            }
        }

        [HttpPost]
        public void CreateMealPlan(MealPlanBindingModel model)
        {
            try
            {
                _mealPlan.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания плана питания");
                throw;
            }
        }

        [HttpPost]
        public void UpdateMealPlan(MealPlanBindingModel model)
        {
            try
            {
                model.MealPlanMembers = null!;
                _mealPlan.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }

        [HttpGet]
        public Tuple<MealPlanViewModel, List<Tuple<string, string>>, List<Tuple<string, string>>>? GetMealPlan(int mealPlanId)
        {
            try
            {
                using var context = new HotelDataBase();
                var elem = _mealPlan.ReadElement(new MealPlanSearchModel { Id = mealPlanId });
                if (elem == null)
                    return null;
                return Tuple.Create(elem, elem.MealPlanMembers.Select(x => Tuple.Create(x.Value.MemberFIO, x.Value.Citizenship)).ToList(), context.Rooms.Where(x => x.MealPlanId == elem.Id).Select(x => Tuple.Create(x.RoomName, x.RoomFrame)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения плана питания по id={Id}", mealPlanId);
                throw;
            }
        }

        [HttpPost]
        public void DeleteMealPlan(MealPlanBindingModel model)
        {
            try
            {
                _mealPlan.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления плана питания");
                throw;
            }
        }

        [HttpPost]
        public void AddMemberToMealPlan(Tuple<MealPlanSearchModel, MemberViewModel> model)
        {
            try
            {
                _mealPlan.AddMemberToMealPlan(model.Item1, model.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка добавления участника в план питания.");
                throw;
            }
        }

        [HttpPost]
        public void CreateRoom(RoomBindingModel model)
        {
            try
            {
                _room.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания комнаты");
                throw;
            }
        }

        [HttpPost]
        public void CreateConferenceBooking(ConferenceBookingBindingModel model)
        {
            try
            {
                _conferenceBooking.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания бронирования конференций");
                throw;
            }
        }

        [HttpPost]
        public void UpdateRoom(RoomBindingModel model)
        {
            try
            {
                model.RoomDinners = null!;
                _room.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }

        [HttpGet]
        public RoomViewModel GetRoomById(int roomId)
        {
            try
            {
                var elem = _room.ReadElement(new RoomSearchModel { Id = roomId });
                if (elem == null)
                    return null;
                return elem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения комнаты по id={Id}", roomId);
                throw;
            }
        }

        [HttpGet]
        public Tuple<RoomViewModel, List<Tuple<string, double>>>? GetRoom(int roomId)
        {
            try
            {
                var elem = _room.ReadElement(new RoomSearchModel { Id = roomId });
                if (elem == null)
                    return null;
                return Tuple.Create(elem, elem.RoomDinners.Select(x => Tuple.Create(x.Value.DinnerName, x.Value.DinnerPrice)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения конференции по id={Id}", roomId);
                throw;
            }
        }

        [HttpPost]
        public void DeleteRoom(RoomBindingModel model)
        {
            try
            {
                _room.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления конференции");
                throw;
            }
        }

        [HttpPost]
        public void AddDinnerToRoom(Tuple<RoomSearchModel, DinnerViewModel> model)
        {
            try
            {
                _room.AddDinnerToRoom(model.Item1, model.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка добавления участника в конференцию.");
                throw;
            }
        }

        [HttpPost]
        public void UpdateConferenceBooking(ConferenceBookingBindingModel model)
        {

            try
            {
                model.ConferenceBookingDinners = null!;
                _conferenceBooking.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }

        [HttpGet]
        public Tuple<ConferenceBookingViewModel, List<Tuple<string, double>>>? GetConferenceBooking(int conferenceBookingId)
        {
            try
            {
                var elem = _conferenceBooking.ReadElement(new ConferenceBookingSearchModel { Id = conferenceBookingId });
                if (elem == null)
                    return null;
                return Tuple.Create(elem, elem.ConferenceBookingDinners.Select(x => Tuple.Create(x.Value.DinnerName, x.Value.DinnerPrice)).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения по id={Id}", conferenceBookingId);
                throw;
            }
        }

        [HttpPost]
        public void DeleteConferenceBooking(ConferenceBookingBindingModel model)
        {
            try
            {
                _conferenceBooking.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления плана питания");
                throw;
            }
        }

        [HttpPost]
        public void AddDinnerToConferenceBooking(Tuple<ConferenceBookingSearchModel, DinnerViewModel> model)
        {
            try
            {
                _conferenceBooking.AddDinnerToConferenceBooking(model.Item1, model.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка добавления участника в план питания.");
                throw;
            }
        }

        [HttpGet]
        public List<ConferenceViewModel>? GetConferences()
        {
            try
            {
                return _conference.ReadList(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка конференций");
                throw;
            }
        }

        [HttpGet]
        public ConferenceBookingViewModel GetConferenceBookingById(int conferenceBookingId)
        {
            try
            {
                var elem = _conferenceBooking.ReadElement(new ConferenceBookingSearchModel { Id = conferenceBookingId });
                if (elem == null)
                    return null;
                return elem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения по id={Id}", conferenceBookingId);
                throw;
            }
        }
    }
}
