using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.StoragesContracts;
using HotelContracts.ViewModels;
using HotelDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDataBaseImplement.Implemets
{
    public class DinnerStorage : IDinnerStorage
    {
        public DinnerViewModel? Delete(DinnerBindingModel model)
        {
            using var context = new HotelDataBase();

            var element = context.Dinners.FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                context.Dinners.Remove(element);
                context.SaveChanges();

                return element.GetViewModel;
            }

            return null;
        }

        public DinnerViewModel? GetElement(DinnerSearchModel model)
        {
            if (string.IsNullOrEmpty(model.DinnerName) && !model.Id.HasValue)
            {
                return null;
            }

            using var context = new HotelDataBase();

            return context.Dinners
                .Include(x => x.RoomDinners)
                .ThenInclude(x => x.Room)
                .Include(x => x.ConferenceBookingDinner)
                .ThenInclude(x => x.ConferenceBooking)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(model.DinnerName) && x.DinnerName == model.DinnerName) || (model.Id.HasValue && x.Id == model.Id))?
                .GetViewModel;
        }

        public List<DinnerViewModel> GetFilteredList(DinnerSearchModel model)
        {
            if (string.IsNullOrEmpty(model.DinnerName) && !model.HeadwaiterId.HasValue)
            {
                return new();
            }

            using var context = new HotelDataBase();

            if (model.HeadwaiterId.HasValue)
            {
                return context.Dinners
                .Include(x => x.RoomDinners)
                .ThenInclude(x => x.Room)
                .Include(x => x.ConferenceBookingDinner)
                .ThenInclude(x => x.ConferenceBooking)
                .Include(x => x.Headwaiter)
                    .Where(x => x.HeadwaiterId == model.HeadwaiterId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }

            return context.Dinners
                .Include(x => x.RoomDinners)
                .ThenInclude(x => x.Room)
                .Include(x => x.ConferenceBookingDinner)
                .ThenInclude(x => x.ConferenceBooking)
                .Include(x => x.Headwaiter)
                .Where(x => x.DinnerName.Contains(model.DinnerName))
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<DinnerViewModel> GetFullList()
        {
            using var context = new HotelDataBase();

            return context.Dinners
            .Include(x => x.RoomDinners)
            .ThenInclude(x => x.Room)
            .Include(x => x.ConferenceBookingDinner)
            .ThenInclude(x => x.ConferenceBooking)
            .Include(x => x.Headwaiter)
            .Select(x => x.GetViewModel)
            .ToList();
        }

        public DinnerViewModel? Insert(DinnerBindingModel model)
        {
            using var context = new HotelDataBase();

            var newDinner = Dinner.Create(model);

            if (newDinner == null)
            {
                return null;
            }

            context.Dinners.Add(newDinner);
            context.SaveChanges();

            return newDinner.GetViewModel;
        }

        public DinnerViewModel? Update(DinnerBindingModel model)
        {
            using var context = new HotelDataBase();

            var dinner = context.Dinners.FirstOrDefault(x => x.Id == model.Id);

            if (dinner == null)
            {
                return null;
            }

            dinner.Update(model);
            context.SaveChanges();

            return dinner.GetViewModel;
        }
    }
}
