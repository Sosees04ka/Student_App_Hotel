using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDataBaseImplement.Models
{
    public class RoomDinner
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int DinnerId { get; set; }

        [Required]
        public int Count { get; set; }
        public virtual Room Room { get; set; } = new();
        public virtual Dinner Dinner { get; set; } = new();
    }
}
