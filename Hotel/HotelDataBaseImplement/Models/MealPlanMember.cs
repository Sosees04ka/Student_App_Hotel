using System.ComponentModel.DataAnnotations;

namespace HotelDataBaseImplement.Models
{
    public class MealPlanMember
    {
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int MealPlanId { get; set; }

        public virtual MealPlan MealPlan { get; set; } = new();
        public virtual Member Member { get; set; } = new();
    }
}
