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
    public class ConferenceBookingLogic : IConferenceBookingLogic
    {
        private readonly ILogger _logger;
        private readonly IConferenceBookingStorage _conferenceBookingStorage;
        private readonly AbstractMailWorker _mailWorker;
        private readonly IHeadwaiterLogic _headwaiterLogic;

        public ConferenceBookingLogic(ILogger<ConferenceBookingLogic> logger, IConferenceBookingStorage conferenceBookingStorage, IHeadwaiterLogic headwaiterLogic, AbstractMailWorker mailWorker)
        {
            _logger = logger;
            _conferenceBookingStorage = conferenceBookingStorage;
            _mailWorker = mailWorker;
            _headwaiterLogic = headwaiterLogic;
        }

        public bool AddDinnerToConferenceBooking(ConferenceBookingSearchModel model, IDinnerModel dinner)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("AddDinnerToConferenceBooking. NameHall:{NameHall}.Id:{ Id}", model.NameHall, model.Id);
            var element = _conferenceBookingStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("AddDinnerToConferenceBooking element not found");
                return false;
            }

            _logger.LogInformation("AddDinnerToConferenceBooking find. Id:{Id}", element.Id);

            element.ConferenceBookingDinners[dinner.Id] = dinner;

            _conferenceBookingStorage.Update(new()
            {
                Id = element.Id,
                NameHall = element.NameHall,
                BookingDate = element.BookingDate,
                ConferenceId = element.ConferenceId,
                HeadwaiterId = element.HeadwaiterId,
                ConferenceBookingDinners = element.ConferenceBookingDinners
            });

            return true;
        }

        public bool Create(ConferenceBookingBindingModel model)
        {
            CheckModel(model);
            model.ConferenceBookingDinners = new();

            var result = _conferenceBookingStorage.Insert(model);

            if (result == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            SendConferenceBookingMessage(result.HeadwaiterId, $"Гостиница \"Развитие\", План питания №{result.Id}", $"План питания №{result.Id} под названием {result.NameHall} и стоимостью {result.BookingDate} добавлен");

            return true;
        }

        public bool Delete(ConferenceBookingBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            var result = _conferenceBookingStorage.Delete(model);

            if (result == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            SendConferenceBookingMessage(result.HeadwaiterId, $"Гостиница \"Развитие\", План питания №{result.Id}", $"План питания №{result.Id} под названием {result.NameHall} и стоимостью {result.BookingDate} удален");

            return true;
        }

        public ConferenceBookingViewModel? ReadElement(ConferenceBookingSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. Id:{Id}", model.Id);

            var element = _conferenceBookingStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);

            return element;
        }

        public List<ConferenceBookingViewModel>? ReadList(ConferenceBookingSearchModel? model)
        {
            _logger.LogInformation("ReadList. Id:{ Id}", model?.Id);

            var list = model == null ? _conferenceBookingStorage.GetFullList() : _conferenceBookingStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool Update(ConferenceBookingBindingModel model)
        {
            CheckModel(model);

            if (_conferenceBookingStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }

        private void CheckModel(ConferenceBookingBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.NameHall))
            {
                throw new ArgumentNullException("Нет названия конференции", nameof(model.NameHall));
            }

            _logger.LogInformation("ConferenceBooking. Id: { Id}",  model.Id);

        }

        private bool SendConferenceBookingMessage(int headwaiterId, string subject, string text)
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
