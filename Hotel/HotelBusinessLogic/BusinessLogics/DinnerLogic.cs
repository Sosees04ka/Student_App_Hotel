using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelBusinessLogic.MailWorker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBusinessLogic.BusinessLogics
{
    public class DinnerLogic : IDinnerLogic
    {
        private readonly ILogger _logger;
        private readonly IDinnerStorage _dinnerStorage;
        private readonly AbstractMailWorker _mailWorker;
        private readonly IHeadwaiterLogic _headwaiterLogic;

        public DinnerLogic(ILogger<DinnerLogic> logger, IDinnerStorage dinnerStorage, AbstractMailWorker mailWorker, IHeadwaiterLogic headwaiterLogic)
        {
            _logger = logger;
            _dinnerStorage = dinnerStorage;
            _mailWorker = mailWorker;
            _headwaiterLogic = headwaiterLogic;
        }

        public bool Create(DinnerBindingModel model)
        {
            CheckModel(model);

            var result = _dinnerStorage.Insert(model);

            if (result == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            SendDinnerMessage(result.HeadwaiterId, $"Гостиница \"Развитие\", Участник №{result.Id}", $"Участник №{result.Id} по имени {result.DinnerName} и с гражданством {result.DinnerPrice} добавлен");

            return true;
        }

        public bool Delete(DinnerBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            var result = _dinnerStorage.Delete(model);

            if (result == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            SendDinnerMessage(result.HeadwaiterId, $"Гостиница \"Развитие\", Участник №{result.Id}", $"Участник №{result.Id} по имени {result.DinnerName} и с гражданством {result.DinnerPrice} удален");

            return true;
        }

        public DinnerViewModel? ReadElement(DinnerSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. DinnerName:{DinnerName}.Id:{Id}", model.DinnerName, model.Id);

            var element = _dinnerStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);

            return element;
        }

        public List<DinnerViewModel>? ReadList(DinnerSearchModel? model)
        {
            _logger.LogInformation("ReadList. DinnerName:{DinnerName}.Id:{ Id}", model?.DinnerName, model?.Id);

            var list = model == null ? _dinnerStorage.GetFullList() : _dinnerStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool Update(DinnerBindingModel model)
        {
            CheckModel(model);

            if (_dinnerStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }
        private void CheckModel(DinnerBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.DinnerName))
            {
                throw new ArgumentNullException("Нет имени обеда", nameof(model.DinnerName));
            }

            if (model.DinnerPrice < 0)
            {
                throw new ArgumentNullException("Стоимость обеда не может быть меньше 0", nameof(model.DinnerPrice));
            }

            _logger.LogInformation("Dinner. DinnerName:{DinnerName}.DinnerPrice:{ DinnerPrice}. Id: { Id}", model.DinnerName, model.DinnerPrice, model.Id);
        }

        private bool SendDinnerMessage(int headwaiterId, string subject, string text)
        {
            var headwaiter = _headwaiterLogic.ReadElement(new() { Id = headwaiterId });

            if (headwaiter == null)
            {
                return false;
            }

            _mailWorker.MailSendAsync(new()
            {
                MailAddress = headwaiter.HeadwaiterEmail,
                Subject = subject,
                Text = text
            });

            return true;
        }
    }
}
