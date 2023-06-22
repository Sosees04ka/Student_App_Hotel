using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelDataBaseImplement.Implemets
{
    public class OrganiserStorage : IOrganiserStorage
    {
        public OrganiserViewModel? Delete(OrganiserBindingModel model)
        {
            using var context = new HotelDataBase();

            var element = context.Organisers.FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.Organisers.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }

        public OrganiserViewModel? GetElement(OrganiserSearchModel model)
        {
            using var context = new HotelDataBase();
            
            if (model.Id.HasValue)
                return context.Organisers
                    .Include(x => x.MealPlans)
                    .Include(x => x.Members)
                    .Include(x => x.Conferences)
                    .FirstOrDefault(x => x.Id == model.Id)?
                    .GetViewModel;
            
            if (!string.IsNullOrEmpty(model.OrganiserEmail) && !string.IsNullOrEmpty(model.OrganiserPassword))
                return context.Organisers
                    .Include(x => x.MealPlans)
                    .Include(x => x.Members)
                    .Include(x => x.Conferences)
                    .FirstOrDefault(x => x.OrganiserEmail.Equals(model.OrganiserEmail) && x.OrganiserPassword.Equals(model.OrganiserPassword))?
                    .GetViewModel;
            
            if (!string.IsNullOrEmpty(model.OrganiserEmail))
                return context.Organisers
                    .Include(x => x.MealPlans)
                    .Include(x => x.Members)
                    .Include(x => x.Conferences)
                    .FirstOrDefault(x => x.OrganiserEmail.Equals(model.OrganiserEmail))?
                    .GetViewModel;
            
            return null;
        }

        public List<OrganiserViewModel> GetFilteredList(OrganiserSearchModel model)
        {
            if (string.IsNullOrEmpty(model.OrganiserFIO))
            {
                return new();
            }

            using var context = new HotelDataBase();

            return context.Organisers
                .Include(x => x.MealPlans)
                .Include(x => x.Members)
                .Include(x => x.Conferences)
                .Where(x => x.OrganiserLogin.Contains(model.OrganiserLogin) && x.OrganiserPassword == model.OrganiserPassword)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<OrganiserViewModel> GetFullList()
        {
            using var context = new HotelDataBase();

            return context.Organisers
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public OrganiserViewModel? Insert(OrganiserBindingModel model)
        {
            var newOrganiser = Organiser.Create(model);

            if (newOrganiser == null)
            {
                return null;
            }

            using var context = new HotelDataBase();

            context.Organisers.Add(newOrganiser);
            context.SaveChanges();

            return newOrganiser.GetViewModel;
        }

        public OrganiserViewModel? Update(OrganiserBindingModel model)
        {
            using var context = new HotelDataBase();

            var organiser = context.Organisers
                .FirstOrDefault(x => x.Id == model.Id);

            if (organiser == null)
            {
                return null;
            }

            organiser.Update(model);
            context.SaveChanges();

            return organiser.GetViewModel;
        }
    }
}
