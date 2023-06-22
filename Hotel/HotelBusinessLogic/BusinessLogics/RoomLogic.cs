using HotelBusinessLogic.MailWorker;
using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBusinessLogic.BusinessLogics
{
    public class RoomLogic : IRoomLogic
    {
        private readonly ILogger _logger;
        private readonly IRoomStorage _roomStorage;
        private readonly AbstractMailWorker _mailWorker;
        private readonly IHeadwaiterLogic _headwaiterLogic;

        public RoomLogic(ILogger<RoomLogic> logger, IRoomStorage roomStorage, IHeadwaiterLogic headwaiterLogic, AbstractMailWorker mailWorker)
        {
            _logger = logger;
            _roomStorage = roomStorage;
            _mailWorker = mailWorker;
            _headwaiterLogic = headwaiterLogic;
        }
        public bool Create(RoomBindingModel model)
        {
            CheckModel(model);
            model.RoomDinners = new();

            var result = _roomStorage.Insert(model);

            if (result == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            SendRoomMessage(result.HeadwaiterId, $"Гостиница \"Развитие\", Конференция №{result.Id}", $"Конференция №{result.Id} под названием {result.RoomName} и датой начала {result.RoomPrice} добавлена {result.RoomFrame}");

            return true;
        }

        public bool Delete(RoomBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            var result = _roomStorage.Delete(model);

            if (result == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            SendRoomMessage(result.HeadwaiterId, $"Гостиница \"Развитие\", Конференция №{result.Id}", $"Конференция №{result.Id} под названием {result.RoomName} и датой начала {result.RoomPrice} удалена {result.RoomFrame}");


            return true;
        }

        public RoomViewModel? ReadElement(RoomSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. RoomName:{RoomName}.Id:{Id}", model.RoomName, model.Id);

            var element = _roomStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);

            return element;
        }

        public List<RoomViewModel>? ReadList(RoomSearchModel? model)
        {
            _logger.LogInformation("ReadList. RoomName:{RoomName}.Id:{ Id}", model?.RoomName, model?.Id);

            var list = model == null ? _roomStorage.GetFullList() : _roomStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool AddDinnerToRoom(RoomSearchModel model, IDinnerModel dinner)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("AddDinnerToRoom. RoomName:{RoomName}.Id:{ Id}", model.RoomName, model.Id);
            var element = _roomStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("AddDinnerToRoom element not found");
                return false;
            }

            _logger.LogInformation("AddDinnerToRoom find. Id:{Id}", element.Id);

            element.RoomDinners[dinner.Id] = dinner;

            _roomStorage.Update(new()
            {
                Id = element.Id,
                RoomName = element.RoomName,
                RoomPrice = element.RoomPrice,
                RoomFrame = element.RoomFrame,
                MealPlanId = element.MealPlanId,
                HeadwaiterId = element.HeadwaiterId,
                RoomDinners = element.RoomDinners,
            });

            return true;
        }

        public bool Update(RoomBindingModel model)
        {
            CheckModel(model);

            if (_roomStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }

        private void CheckModel(RoomBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.RoomName))
            {
                throw new ArgumentNullException("Нет названия комнате", nameof(model.RoomName));
            }


            if (string.IsNullOrEmpty(model.RoomFrame))
            {
                throw new ArgumentNullException("Нет названия корпусу", nameof(model.RoomFrame));
            }

            if (model.RoomPrice < 0)
            {
                throw new ArgumentNullException("Стоимость комнаты не может быть меньше 0", nameof(model.RoomPrice));
            }

            _logger.LogInformation("Room. RoomName:{RoomName}.RoomFrame:{ RoomFrame}.RoomPrice:{ RoomPrice}. Id: { Id}", model.RoomName, model.RoomFrame, model.RoomPrice, model.Id);
        }

        private bool SendRoomMessage(int headwaiterId, string subject, string text)
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
