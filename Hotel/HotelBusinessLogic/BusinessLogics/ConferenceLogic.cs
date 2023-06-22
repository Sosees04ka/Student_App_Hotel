using HotelBusinessLogic.MailWorker;
using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using Microsoft.Extensions.Logging;
using System.IO.Packaging;

namespace HotelBusinessLogic.BusinessLogics
{
    public class ConferenceLogic : IConferenceLogic
    {
        private readonly ILogger _logger;
        private readonly IConferenceStorage _conferenceStorage;
        private readonly AbstractMailWorker _mailWorker;
        private readonly IOrganiserLogic _organiserLogic;

        public ConferenceLogic(ILogger<ConferenceLogic> logger, IConferenceStorage conferenceStorage, IOrganiserLogic organiserLogic, AbstractMailWorker mailWorker)
        {
            _logger = logger;
            _conferenceStorage = conferenceStorage;
            _organiserLogic = organiserLogic;
            _mailWorker = mailWorker;
        }
        public bool Create(ConferenceBindingModel model)
        {
            CheckModel(model);
            model.ConferenceMembers = new();

            var result = _conferenceStorage.Insert(model);

            if (result == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            SendConferenceMessage(result.OrganiserId, $"Гостиница \"Развитие\", Конференция №{result.Id}", $"Конференция №{result.Id} под названием {result.ConferenceName} и датой начала {result.StartDate} добавлена");

            return true;
        }

        public bool Delete(ConferenceBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            var result = _conferenceStorage.Delete(model);

            if (result == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            SendConferenceMessage(result.OrganiserId, $"Гостиница \"Развитие\", Конференция №{result.Id}", $"Конференция №{result.Id} под названием {result.ConferenceName} и датой начала {result.StartDate} удалена");

            return true;
        }

        public ConferenceViewModel? ReadElement(ConferenceSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. ConferenceName:{ConferenceName}.Id:{Id}", model.ConferenceName, model.Id);

            var element = _conferenceStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);

            return element;
        }

        public List<ConferenceViewModel>? ReadList(ConferenceSearchModel? model)
        {
            _logger.LogInformation("ReadList. ConferenceName:{ConferenceName}.Id:{ Id}", model?.ConferenceName, model?.Id);

            var list = model == null ? _conferenceStorage.GetFullList() : _conferenceStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool AddMemberToConference(ConferenceSearchModel model, IMemberModel member)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("AddMemberToConference. ConferenceName:{ConferenceName}.Id:{ Id}", model.ConferenceName, model.Id);
            var element = _conferenceStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("AddMemberToConference element not found");
                return false;
            }

            _logger.LogInformation("AddMemberToConference find. Id:{Id}", element.Id);

            element.ConferenceMembers[member.Id] = member;

            _conferenceStorage.Update(new()
            {
                Id = element.Id,
                ConferenceName = element.ConferenceName,
                StartDate = element.StartDate,
                OrganiserId = element.OrganiserId,
                ConferenceMembers = element.ConferenceMembers,
            });

            return true;
        }


        public bool Update(ConferenceBindingModel model)
        {
            CheckModel(model);

            if (_conferenceStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }

        private void CheckModel(ConferenceBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.ConferenceName))
            {
                throw new ArgumentNullException("Нет названия конференции", nameof(model.ConferenceName));
            }

            _logger.LogInformation("Conference. ConferenceName:{ConferenceName}.StartDate:{ StartDate}. Id: { Id}", model.ConferenceName, model.StartDate, model.Id);
        }

        private bool SendConferenceMessage(int organiserId, string subject, string text)
        {
            var organiser = _organiserLogic.ReadElement(new() { Id = organiserId });

            if (organiser == null)
            {
                return false;
            }

            _mailWorker.MailSendAsync(new()
            {
                MailAddress = organiser.OrganiserEmail,
                Subject = subject,
                Text = text
            });

            return true;
        }
    }
}
