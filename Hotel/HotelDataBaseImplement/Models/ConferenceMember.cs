using System.ComponentModel.DataAnnotations;

namespace HotelDataBaseImplement.Models
{
    public class ConferenceMember
    {
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int ConferenceId { get; set; }

        public virtual Conference Conference { get; set; } = new();
        public virtual Member Member { get; set; } = new();
    }
}
