using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelDataBaseImplement.Implemets
{
    public class MealPlanStorage : IMealPlanStorage
    {
        public MealPlanViewModel? Delete(MealPlanBindingModel model)
        {
            using var context = new HotelDataBase();

            var element = context.MealPlans
                .Include(x => x.Members)
                .FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.MealPlans.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }

        public MealPlanViewModel? GetElement(MealPlanSearchModel model)
        {
            if (string.IsNullOrEmpty(model.MealPlanName) && !model.Id.HasValue)
            {
                return null;
            }

            using var context = new HotelDataBase();

            return context.MealPlans
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.ConferenceMembers)
                .ThenInclude(x => x.Conference)
                .Include(x => x.Rooms)
                .Include(x => x.Organiser)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(model.MealPlanName) && x.MealPlanName == model.MealPlanName) || (model.Id.HasValue && x.Id == model.Id))?
                .GetViewModel;
        }

        public List<MealPlanViewModel> GetFilteredList(MealPlanSearchModel model)
        {
            if (string.IsNullOrEmpty(model.MealPlanName) && !model.OrganiserId.HasValue)
            {
                return new();
            }

            using var context = new HotelDataBase();

            if (model.OrganiserId.HasValue)
            {
                return context.MealPlans
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.ConferenceMembers)
                .ThenInclude(x => x.Conference)
                .Include(x => x.Rooms)
                .Include(x => x.Organiser)
                    .Where(x => x.OrganiserId == model.OrganiserId)
                    .ToList()
                    .Select(x => x.GetViewModel)
                    .ToList();
            }

            return context.MealPlans
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.ConferenceMembers)
                .ThenInclude(x => x.Conference)
                .Include(x => x.Rooms)
                .Include(x => x.Organiser)
                .Where(x => x.MealPlanName.Contains(model.MealPlanName))
                .ToList()
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<MealPlanViewModel> GetFullList()
        {
            using var context = new HotelDataBase();
            return context.MealPlans
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.ConferenceMembers)
                .ThenInclude(x => x.Conference)
                .Include(x => x.Rooms)
                .Include(x => x.Organiser)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public MealPlanViewModel? Insert(MealPlanBindingModel model)
        {
            using var context = new HotelDataBase();
            var newMealPlan = MealPlan.Create(context,model);

            if (newMealPlan == null)
            {
                return null;
            }
            
            context.MealPlans.Add(newMealPlan);
            context.SaveChanges();
            return newMealPlan.GetViewModel;
        }

        public MealPlanViewModel? Update(MealPlanBindingModel model)
        {
            using var context = new HotelDataBase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var elem = context.MealPlans.FirstOrDefault(rec => rec.Id == model.Id);
                if (elem == null)
                {
                    return null;
                }
                elem.Update(model);
                context.SaveChanges();
                if (model.MealPlanMembers != null)
                    elem.UpdateMembers(context, model);
                transaction.Commit();
                return elem.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
