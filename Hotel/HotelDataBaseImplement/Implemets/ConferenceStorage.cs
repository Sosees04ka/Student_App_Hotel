using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelDataBaseImplement.Implemets
{
    public class ConferenceStorage : IConferenceStorage
    {
        public ConferenceViewModel? Delete(ConferenceBindingModel model)
        {
            using var context = new HotelDataBase();

            var element = context.Conferences
                .Include(x => x.Members)
                .FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.Conferences.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }

        public ConferenceViewModel? GetElement(ConferenceSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ConferenceName) && !model.Id.HasValue)
            {
                return null;
            }

            using var context = new HotelDataBase();

            return context.Conferences
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.MealPlanMember)
                .ThenInclude(x => x.MealPlan)
                .Include(x => x.ConferenceBookings)
                .Include(x => x.Organiser)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(model.ConferenceName) && x.ConferenceName == model.ConferenceName) || (model.Id.HasValue && x.Id == model.Id))?
                .GetViewModel;
        }

        public List<ConferenceViewModel> GetFilteredList(ConferenceSearchModel model)
        {
            if (!model.DateFrom.HasValue && !model.DateTo.HasValue && !model.OrganiserId.HasValue)
            {
                return new();
            }
            using var context = new HotelDataBase();

            if (model.DateFrom.HasValue)
            {
                return context.Conferences
                        .Include(x => x.Members)
                        .ThenInclude(x => x.Member)
                        .ThenInclude(x => x.MealPlanMember)
                        .ThenInclude(x => x.MealPlan)
                        .Include(x => x.ConferenceBookings)
                        .Include(x => x.Organiser)
                        .Where(x => x.StartDate >= model.DateFrom && x.StartDate <= model.DateTo && x.OrganiserId == model.OrganiserId)
                        .Select(x => x.GetViewModel)
                        .ToList();
            }
            else if (model.OrganiserId.HasValue)
                return context.Conferences
                    .Include(x => x.Members)
                    .ThenInclude(x => x.Member)
                    .ThenInclude(x => x.MealPlanMember)
                    .ThenInclude(x => x.MealPlan)
                    .Include(x => x.ConferenceBookings)
                    .Include(x => x.Organiser)
                    .Where(x => x.OrganiserId == model.OrganiserId)
                    .ToList()
                    .Select(x => x.GetViewModel)
                    .ToList();

            return context.Conferences
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.MealPlanMember)
                .ThenInclude(x => x.MealPlan)
                .Include(x => x.ConferenceBookings)
                .Include(x => x.Organiser)
                .Where(x => x.ConferenceName.Contains(model.ConferenceName))
                .ToList()
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<ConferenceViewModel> GetFullList()
        {
            using var context = new HotelDataBase();

            return context.Conferences
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.MealPlanMember)
                .ThenInclude(x => x.MealPlan)
                .Include(x => x.ConferenceBookings)
                .Include(x => x.Organiser)
                .ToList()
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public ConferenceViewModel? Insert(ConferenceBindingModel model)
        {
            using var context = new HotelDataBase();
            var newConference = Conference.Create(context,model);

            if (newConference == null)
            {
                return null;
            }

            context.Conferences.Add(newConference);
            context.SaveChanges();

            return context.Conferences
                .Include(x => x.Members)
                .ThenInclude(x => x.Member)
                .ThenInclude(x => x.MealPlanMember)
                .ThenInclude(x => x.MealPlan)
                .Include(x => x.ConferenceBookings)
                .Include(x => x.Organiser)
                .FirstOrDefault(x => x.Id == newConference.Id)
                ?.GetViewModel;
        }
    

        public ConferenceViewModel? Update(ConferenceBindingModel model)
        {
            using var context = new HotelDataBase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var elem = context.Conferences.FirstOrDefault(rec => rec.Id == model.Id);
                if (elem == null)
                {
                    return null;
                }
                elem.Update(model);
                context.SaveChanges();
                if (model.ConferenceMembers != null)
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
