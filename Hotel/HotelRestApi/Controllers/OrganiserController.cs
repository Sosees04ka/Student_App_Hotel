using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganiserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOrganiserLogic _logic;
        public OrganiserController(IOrganiserLogic logic, ILogger<OrganiserController> logger)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet]
        public OrganiserViewModel? Login(string login, string password)
        {
            try
            {
                return _logic.ReadElement(new OrganiserSearchModel
                {
                    OrganiserEmail = login,
                    OrganiserPassword = password
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка входа в систему");
                throw;
            }
        }

        [HttpPost]
        public void Register(OrganiserBindingModel model)
        {
            try
            {
                _logic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка регистрации");
                throw;
            }
        }

        [HttpPost]
        public void UpdateData(OrganiserBindingModel model)
        {
            try
            {
                _logic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }
    }
}
