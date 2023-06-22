using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotelBusinessLogic.BusinessLogics
{
    public class HeadwaiterLogic : IHeadwaiterLogic
    {
        private readonly int _loginMaxLength = 50;
        private readonly int _passwordMaxLength = 50;
        private readonly int _passwordMinLength = 10;

        private readonly ILogger _logger;
        private readonly IHeadwaiterStorage _headwaiterStorage;

        public HeadwaiterLogic(ILogger<HeadwaiterLogic> logger, IHeadwaiterStorage headwaiterStorage)
        {
            _logger = logger;
            _headwaiterStorage = headwaiterStorage;
        }

        public bool Create(HeadwaiterBindingModel model)
        {
            CheckModel(model);

            if (_headwaiterStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");

                return false;
            }

            return true;
        }

        public bool Delete(HeadwaiterBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id: {Id}", model.Id);

            if (_headwaiterStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");

                return false;
            }

            return true;
        }

        public HeadwaiterViewModel? ReadElement(HeadwaiterSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. HeadwaiterFIO: {HeadwaiterFIO}. HeadwaiterLogin: {HeadwaiterLogin}. Id: {Id}.", model.HeadwaiterFIO, model.HeadwaiterLogin, model.Id);

            var element = _headwaiterStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id: {Id}", element.Id);

            return element;
        }

        public List<HeadwaiterViewModel>? ReadList(HeadwaiterSearchModel? model)
        {
            _logger.LogInformation("ReadList. HeadwaiterFIO: {HeadwaiterFIO}. HeadwaiterLogin: {HeadwaiterLogin}. Id: {Id}.", model?.HeadwaiterFIO, model?.HeadwaiterLogin, model?.Id);

            var list = model == null ? _headwaiterStorage.GetFullList() : _headwaiterStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count: {Count}", list.Count);

            return list;
        }

        public bool Update(HeadwaiterBindingModel model)
        {
            CheckModel(model);

            if (_headwaiterStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(HeadwaiterBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.HeadwaiterFIO))
            {
                throw new ArgumentNullException("Нет ФИО метродотеля", nameof(model.HeadwaiterFIO));
            }

            if (string.IsNullOrEmpty(model.HeadwaiterLogin))
            {
                throw new ArgumentNullException("Нет логина метродотеля", nameof(model.HeadwaiterLogin));
            }

            if (model.HeadwaiterLogin.Length > _loginMaxLength)
            {
                throw new ArgumentNullException("Логин слишком длинный", nameof(model.HeadwaiterLogin));
            }

            if (string.IsNullOrEmpty(model.HeadwaiterNumber))
            {
                throw new ArgumentNullException("Нет номера телефона метродотеля", nameof(model.HeadwaiterNumber));
            }

            if (model.HeadwaiterEmail.Length > _loginMaxLength || !Regex.IsMatch(model.HeadwaiterEmail, @"([a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z0-9]+)"))
            {
                throw new Exception($"В качестве логина должна быть указана почта и иметь длинну не более {_loginMaxLength} символов");
            }

            if (string.IsNullOrEmpty(model.HeadwaiterEmail))
            {
                throw new ArgumentNullException("Нет почты метродотеля", nameof(model.HeadwaiterEmail));
            }

            if (string.IsNullOrEmpty(model.HeadwaiterPassword))
            {
                throw new ArgumentNullException("Нет пароля метродотеля", nameof(model.HeadwaiterPassword));
            }

            if (model.HeadwaiterPassword.Length > _passwordMaxLength || model.HeadwaiterPassword.Length < _passwordMinLength
                           || !Regex.IsMatch(model.HeadwaiterPassword, @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
            {
                throw new Exception($"Пароль длиной от {_passwordMinLength} до {_passwordMaxLength} должен состоять из цифр, букв и небуквенных символов");
            }

            _logger.LogInformation("Headwaiter. HeadwaiterFIO: {HeadwaiterFIO}. HeadwaiterLogin: {HeadwaiterLogin}. Id: {Id}", model.HeadwaiterFIO, model.HeadwaiterLogin, model.Id);

            var element = _headwaiterStorage.GetElement(new HeadwaiterSearchModel
            {
                HeadwaiterEmail = model.HeadwaiterEmail
            });

            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("метродотель с таким логином уже есть");
            }
        }
    }
}
