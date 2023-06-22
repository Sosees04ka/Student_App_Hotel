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
    public class Dinner : IDinnerModel
    {
        public int Id { get; set; }
        public int HeadwaiterId { get; set; }

        [Required]
        public string DinnerName { get; set; } = string.Empty;

        [Required]
        public double DinnerPrice { get; set; }

        public virtual Headwaiter Headwaiter { get; set; }

        [ForeignKey("DinnerId")]
        public virtual List<RoomDinner> RoomDinners { get; set; } = new();

        [ForeignKey("DinnerId")]
        public virtual List<ConferenceBookingDinner> ConferenceBookingDinner { get; set; } = new();
        public static Dinner? Create(DinnerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Dinner()
            {
                Id = model.Id,
                DinnerName = model.DinnerName,
                HeadwaiterId = model.HeadwaiterId,
                DinnerPrice = model.DinnerPrice
            };
        }
        public static Dinner Create(DinnerViewModel model)
        {
            return new Dinner
            {
                Id = model.Id,
                DinnerName = model.DinnerName,
                HeadwaiterId = model.HeadwaiterId,
                DinnerPrice = model.DinnerPrice
            };
        }
        public void Update(DinnerBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            HeadwaiterId = model.HeadwaiterId;
            DinnerName = model.DinnerName;
            DinnerPrice = model.DinnerPrice;
        }
        public DinnerViewModel GetViewModel => new()
        {
            Id = Id,
            DinnerName = DinnerName,
            HeadwaiterId = HeadwaiterId,
            DinnerPrice = DinnerPrice
        };
    }
}
