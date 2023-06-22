using HotelContracts.BindingModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDataBaseImplement.Models
{
    public class ConferenceBooking : IConferenceBookingModel
    {
        public DateTime? BookingDate { get; set; }
        public int HeadwaiterId { get; private set; }
        public int? ConferenceId { get; private set; }
        public int Id { get; private set; }
        public string NameHall { get; set; } = string.Empty;
        public virtual Headwaiter Headwaiter { get; set; }
        public virtual Conference? Conference { get; set; }

        [ForeignKey("ConferenceBookingId")]
        public virtual List<ConferenceBookingDinner> Dinners { get; set; }

        private Dictionary<int, IDinnerModel> _conferenceBookingDinners = null;

        [NotMapped]
        public Dictionary<int, IDinnerModel> ConferenceBookingDinners
        {
            get
            {
                if (_conferenceBookingDinners == null)
                {
                    using var context = new HotelDataBase();
                    _conferenceBookingDinners = Dinners
                        .ToDictionary(x => x.DinnerId, x => (context.Dinners
                        .FirstOrDefault(y => y.Id == x.DinnerId)! as IDinnerModel));
                }
                return _conferenceBookingDinners;
            }
        }
        public static ConferenceBooking Create(HotelDataBase context, ConferenceBookingBindingModel model)
        {
            return new ConferenceBooking()
            {
                Id = model.Id,
                ConferenceId = model.ConferenceId,
                HeadwaiterId = model.HeadwaiterId,
                NameHall = model.NameHall,
                Dinners = model.ConferenceBookingDinners.Select(x => new ConferenceBookingDinner
                {
                    Dinner = context.Dinners.First(y => y.Id == x.Key),
                }).ToList()
            };
        }

        public void Update(ConferenceBookingBindingModel model)
        {
            ConferenceId = model.ConferenceId;
            NameHall = model.NameHall;
            BookingDate = model.BookingDate;
        }

        public ConferenceBookingViewModel GetViewModel => new()
        {
            Id = Id,
            ConferenceId = ConferenceId,
            HeadwaiterId = HeadwaiterId,
            NameHall = NameHall,
            BookingDate = BookingDate,
            ConferenceBookingDinners = ConferenceBookingDinners,
            ConfName = Conference?.ConferenceName
        };

        public void UpdateDinners(HotelDataBase context, ConferenceBookingBindingModel model)
        {
            var conferenceBookingDinners = context.ConferenceBookingDinners.Where(rec => rec.ConferenceBookingId == model.Id).ToList();

            if (conferenceBookingDinners != null && conferenceBookingDinners.Count > 0)
            {
                context.ConferenceBookingDinners.RemoveRange(conferenceBookingDinners.Where(rec => !model.ConferenceBookingDinners.ContainsKey(rec.DinnerId)));
                context.SaveChanges();

                foreach (var updateDinner in conferenceBookingDinners)
                {
                    model.ConferenceBookingDinners.Remove(updateDinner.DinnerId);
                }
                context.SaveChanges();
            }

            var conferenceBooking = context.ConferenceBookings.First(x => x.Id == Id);

            foreach (var cm in model.ConferenceBookingDinners)
            {
                context.ConferenceBookingDinners.Add(new ConferenceBookingDinner
                {
                    ConferenceBooking = conferenceBooking,
                    Dinner = context.Dinners.First(x => x.Id == cm.Key)
                });
                context.SaveChanges();
            }
            _conferenceBookingDinners = null;
        }
    }

}

