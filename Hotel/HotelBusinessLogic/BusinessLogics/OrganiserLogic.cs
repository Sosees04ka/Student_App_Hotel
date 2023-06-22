using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace HotelBusinessLogic.BusinessLogics
{
    public class OrganiserLogic : IOrganiserLogic
    {
        private readonly int _loginMaxLength = 50;
        private readonly int _passwordMaxLength = 50;
        private readonly int _passwordMinLength = 10;

        private readonly ILogger _logger;
        private readonly IOrganiserStorage _organiserStorage;

        public OrganiserLogic(ILogger<OrganiserLogic> logger, IOrganiserStorage organiserStorage)
        {
            _logger = logger;
            _organiserStorage = organiserStorage;
        }

        public bool Create(OrganiserBindingModel model)
        {
            CheckModel(model);

            if (_organiserStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");

                return false;
            }

            return true;
        }

        public bool Delete(OrganiserBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id: {Id}", model.Id);

            if (_organiserStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");

                return false;
            }

            return true;
        }

        public OrganiserViewModel? ReadElement(OrganiserSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. OrganiserFIO: {OrganiserFIO}. OrganiserLogin: {OrganiserLogin}. Id: {Id}.", model.OrganiserFIO, model.OrganiserLogin, model.Id);

            var element = _organiserStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id: {Id}", element.Id);

            return element;
        }

        public List<OrganiserViewModel>? ReadList(OrganiserSearchModel? model)
        {
            _logger.LogInformation("ReadList. OrganiserFIO: {OrganiserFIO}. OrganiserLogin: {OrganiserLogin}. Id: {Id}.", model?.OrganiserFIO, model?.OrganiserLogin, model?.Id);

            var list = model == null ? _organiserStorage.GetFullList() : _organiserStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count: {Count}", list.Count);

            return list;
        }

        public bool Update(OrganiserBindingModel model)
        {
            CheckModel(model);

            if (_organiserStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(OrganiserBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.OrganiserFIO))
            {
                throw new ArgumentNullException("Нет ФИО организатора", nameof(model.OrganiserFIO));
            }

            if (string.IsNullOrEmpty(model.OrganiserLogin))
            {
                throw new ArgumentNullException("Нет логина организатора", nameof(model.OrganiserLogin));
            }

            if (model.OrganiserLogin.Length>_loginMaxLength)
            {
                throw new ArgumentNullException("Логин слишком длинный", nameof(model.OrganiserLogin));
            }

            if (string.IsNullOrEmpty(model.OrganiserNumber))
            {
                throw new ArgumentNullException("Нет номера телефона организатора", nameof(model.OrganiserNumber));
            }

            if (model.OrganiserEmail.Length > _loginMaxLength || !Regex.IsMatch(model.OrganiserEmail, @"([a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+)"))
            {
                throw new Exception($"В качестве логина должна быть указана почта и иметь длинну не более {_loginMaxLength} символов");
            }

            if (string.IsNullOrEmpty(model.OrganiserEmail))
            {
                throw new ArgumentNullException("Нет почты организатора", nameof(model.OrganiserEmail));
            }

            if (string.IsNullOrEmpty(model.OrganiserPassword))
            {
                throw new ArgumentNullException("Нет пароля организатора", nameof(model.OrganiserPassword));
            }

            if (model.OrganiserPassword.Length > _passwordMaxLength || model.OrganiserPassword.Length < _passwordMinLength
                || !Regex.IsMatch(model.OrganiserPassword, @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
            {
                throw new Exception($"Пароль длиной от {_passwordMinLength} до {_passwordMaxLength} должен состоять из цифр, букв и небуквенных символов");
            }

            _logger.LogInformation("Organiser. OrganiserFIO: {OrganiserFIO}. OrganiserLogin: {OrganiserLogin}. Id: {Id}", model.OrganiserFIO, model.OrganiserLogin, model.Id);

            var element = _organiserStorage.GetElement(new OrganiserSearchModel
            {
                OrganiserEmail = model.OrganiserEmail
            });

            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Организатор с таким логином уже есть");
            }
        }
    }
}
