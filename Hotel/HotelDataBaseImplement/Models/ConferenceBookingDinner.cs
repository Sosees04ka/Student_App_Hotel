using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDataBaseImplement.Models
{
    public class ConferenceBookingDinner
    {
        public int Id { get; set; }
        public int ConferenceBookingId { get; set; }
        public int DinnerId { get; set; }

        [Required]
        public virtual ConferenceBooking ConferenceBooking { get; set; }
        public virtual Dinner Dinner { get; set; }
    }
}
