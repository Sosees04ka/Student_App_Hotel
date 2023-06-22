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
    public class Headwaiter : IHeadwaiterModel
    {
        public int Id { get; private set; }
        
        [Required]
        public string HeadwaiterFIO { get; set; } = string.Empty;

        [Required]
        public string HeadwaiterEmail { get; set; } = string.Empty;

        [Required]
        public string HeadwaiterPassword { get; set; } = string.Empty;

        [Required]
        public string HeadwaiterLogin { get; set; } = string.Empty;

        [Required]
        public string HeadwaiterNumber { get; set; } = string.Empty;

        
        [ForeignKey("HeadwaiterId")]
        public virtual List<Room> Rooms { get; set; } = new();
        
        [ForeignKey("HeadwaiterId")]
        public virtual List<Dinner> Dinners { get; set; } = new();
        
        [ForeignKey("HeadwaiterId")]
        public virtual List<ConferenceBooking> ConferenceBookings { get; set; } = new();
        public static Headwaiter? Create(HeadwaiterBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Headwaiter()
            {
                Id = model.Id,
                HeadwaiterFIO = model.HeadwaiterFIO,
                HeadwaiterEmail = model.HeadwaiterEmail,
                HeadwaiterPassword = model.HeadwaiterPassword,
                HeadwaiterLogin = model.HeadwaiterLogin,
                HeadwaiterNumber = model.HeadwaiterNumber
            };
        }
        public static Headwaiter Create(HeadwaiterViewModel model)
        {
            return new Headwaiter
            {
                Id = model.Id,
                HeadwaiterFIO = model.HeadwaiterFIO,
                HeadwaiterEmail = model.HeadwaiterEmail,
                HeadwaiterPassword = model.HeadwaiterPassword,
                HeadwaiterLogin = model.HeadwaiterLogin,
                HeadwaiterNumber = model.HeadwaiterNumber
            };
        }
        public void Update(HeadwaiterBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            HeadwaiterFIO = model.HeadwaiterFIO;
            HeadwaiterEmail = model.HeadwaiterEmail;
            HeadwaiterPassword = model.HeadwaiterPassword;
            HeadwaiterLogin = model.HeadwaiterLogin;
            HeadwaiterNumber = model.HeadwaiterNumber;
        }
        public HeadwaiterViewModel GetViewModel => new()
        {
            Id = Id,
            HeadwaiterFIO = HeadwaiterFIO,
            HeadwaiterEmail = HeadwaiterEmail,
            HeadwaiterPassword = HeadwaiterPassword,
            HeadwaiterLogin = HeadwaiterLogin,
            HeadwaiterNumber = HeadwaiterNumber
        };

    }
}
