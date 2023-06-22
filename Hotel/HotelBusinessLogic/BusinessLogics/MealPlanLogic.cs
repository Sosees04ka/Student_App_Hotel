using HotelBusinessLogic.MailWorker;
using HotelContracts.BindingModels;
using HotelContracts.BusinessLogicsContracts;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using Microsoft.Extensions.Logging;

namespace HotelBusinessLogic.BusinessLogics
{
    public class MealPlanLogic : IMealPlanLogic
    {
        private readonly ILogger _logger;
        private readonly IMealPlanStorage _mealPlanStorage;
        private readonly AbstractMailWorker _mailWorker;
        private readonly IOrganiserLogic _organiserLogic;

        public MealPlanLogic(ILogger<MealPlanLogic> logger, IMealPlanStorage mealPlanStorage, IOrganiserLogic organiserLogic, AbstractMailWorker mailWorker)
        {
            _logger = logger;
            _mealPlanStorage = mealPlanStorage;
            _organiserLogic = organiserLogic;
            _mailWorker= mailWorker;
        }

        public bool AddMemberToMealPlan(MealPlanSearchModel model, IMemberModel member)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("AddMemberToMealPlan. MealPlanName:{MealPlanName}.Id:{ Id}", model.MealPlanName, model.Id);
            var element = _mealPlanStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("AddMemberToMealPlan element not found");
                return false;
            }

            _logger.LogInformation("AddMemberToMealPlan find. Id:{Id}", element.Id);

            element.MealPlanMembers[member.Id] = member;

            _mealPlanStorage.Update(new()
            {
                Id = element.Id,
                MealPlanName = element.MealPlanName,
                MealPlanPrice = element.MealPlanPrice,
                OrganiserId = element.OrganiserId,
                MealPlanMembers = element.MealPlanMembers 
            });

            return true;
        }

        public bool Create(MealPlanBindingModel model)
        {
            CheckModel(model);
            model.MealPlanMembers = new();

            var result = _mealPlanStorage.Insert(model);

            if (result == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }

            SendMealPlanMessage(result.OrganiserId, $"Гостиница \"Развитие\", План питания №{result.Id}", $"План питания №{result.Id} под названием {result.MealPlanName} и стоимостью {result.MealPlanPrice} добавлен");

            return true;
        }

        public bool Delete(MealPlanBindingModel model)
        {
            CheckModel(model, false);

            _logger.LogInformation("Delete. Id:{Id}", model.Id);

            var result = _mealPlanStorage.Delete(model);

            if (result== null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }

            SendMealPlanMessage(result.OrganiserId, $"Гостиница \"Развитие\", План питания №{result.Id}", $"План питания №{result.Id} под названием {result.MealPlanName} и стоимостью {result.MealPlanPrice} удален");

            return true;
        }

        public MealPlanViewModel? ReadElement(MealPlanSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogInformation("ReadElement. MealPlanName:{MealPlanName}.Id:{Id}", model.MealPlanName, model.Id);

            var element = _mealPlanStorage.GetElement(model);

            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }

            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);

            return element;
        }

        public List<MealPlanViewModel>? ReadList(MealPlanSearchModel? model)
        {
            _logger.LogInformation("ReadList. MealPlanName:{MealPlanName}.Id:{ Id}", model?.MealPlanName, model?.Id);

            var list = model == null ? _mealPlanStorage.GetFullList() : _mealPlanStorage.GetFilteredList(model);

            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }

            _logger.LogInformation("ReadList. Count:{Count}", list.Count);

            return list;
        }

        public bool Update(MealPlanBindingModel model)
        {
            CheckModel(model);

            if (_mealPlanStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }

            return true;
        }

        private void CheckModel(MealPlanBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (!withParams)
            {
                return;
            }

            if (string.IsNullOrEmpty(model.MealPlanName))
            {
                throw new ArgumentNullException("Нет названия плана питания", nameof(model.MealPlanName));
            }

            if (model.MealPlanPrice<0)
            {
                throw new ArgumentNullException("Стоимость плана питания не может быть меньше 0", nameof(model.MealPlanPrice));
            }

            _logger.LogInformation("MealPlan. MealPlanName:{MealPlanName}.MealPlanPrice:{ MealPlanPrice}. Id: { Id}", model.MealPlanName, model.MealPlanPrice, model.Id);

            var element = _mealPlanStorage.GetElement(new MealPlanSearchModel
            {
                MealPlanName = model.MealPlanName
            });

            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("План питания с таким названием уже есть");
            }
        }

        private bool SendMealPlanMessage(int organiserId, string subject, string text)
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
