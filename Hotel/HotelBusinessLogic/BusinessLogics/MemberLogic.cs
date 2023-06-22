using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using Microsoft.Extensions.Logging;
using HotelBusinessLogic.MailWorker;

namespace HotelBusinessLogic.BusinessLogics
{
    public class MemberLogic : IMemberLogic
    {
        private readonly ILogger _logger;
        private readonly IMemberStorage _memberStorage;
        private readonly AbstractMailWorker _mailWorker;
        private readonly IOrganiserLogic _organiserLogic;

        public MemberLogic(ILogger<MemberLogic> logger, IMemberStorage memberStorage, AbstractMailWorker mailWorker, IOrganiserLogic organiserLogic)
        {
            _logger = logger;
            _memberStorage = memberStorage;
            _mailWorker = mailWorker;
            _organiserLogic = organiserLogic;
        }

        public bool Create(MemberBindingModel model)
        {
            CheckModel(model);

            var result = _memberStorage.Insert(model);

            if (result == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            SendMemberMessage(result.OrganiserId, $"Гостиница \"Развитие\", Участник №{result.Id}", $"Участник №{result.Id} по имени {result.MemberFIO} и с гражданством {result.Citizenship} добавлен");

            return true;
        }

        public bool Delete(MemberBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            var result = _memberStorage.Delete(model);

            if (result == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            SendMemberMessage(result.OrganiserId, $"Гостиница \"Развитие\", Участник №{result.Id}", $"Участник №{result.Id} по имени {result.MemberFIO} и с гражданством {result.Citizenship} удален");

            return true;
        }

        public MemberViewModel? ReadElement(MemberSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. MemberFIO:{MemberFIO}.Id:{Id}", model.MemberFIO, model.Id);

            var element = _memberStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);

            return element;
        }

        public List<MemberViewModel>? ReadList(MemberSearchModel? model)
        {
            _logger.LogInformation("ReadList. MemberFIO:{MemberFIO}.Id:{ Id}", model?.MemberFIO, model?.Id);

            var list = model == null ? _memberStorage.GetFullList() : _memberStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool Update(MemberBindingModel model)
        {
            CheckModel(model);

            if (_memberStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }
        private void CheckModel(MemberBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.MemberFIO))
            {
                throw new ArgumentNullException("Нет ФИО участника", nameof(model.MemberFIO));
            }

            if (string.IsNullOrEmpty(model.Citizenship))
            {
                throw new ArgumentNullException("Не указано гражданство участника", nameof(model.Citizenship));
            }

            _logger.LogInformation("Member. MemberFIO:{MemberFIO}.Citizenship:{ Citizenship}. Id: { Id}", model.MemberFIO, model.Citizenship, model.Id);
        }

        private bool SendMemberMessage(int organiserId, string subject, string text)
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
