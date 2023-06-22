using HotelContracts.BindingModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelDataBaseImplement.Models
{
    public class MealPlan : IMealPlanModel
    {
        [Required]
        public string MealPlanName { get; set; } = string.Empty;

        [Required]
        public double MealPlanPrice { get; set; }

        public int OrganiserId { get; private set; }

        public int Id { get; private set; }

        public virtual Organiser Organiser { get; set; }

        private Dictionary<int, IMemberModel> _mealPlanMembers = null;

        [NotMapped]
        public Dictionary<int, IMemberModel> MealPlanMembers
        {
            get
            {
                if (_mealPlanMembers == null)
                {
                    using var context = new HotelDataBase();
                    _mealPlanMembers = Members
                        .ToDictionary(x => x.MemberId, x => (context.Members
                        .FirstOrDefault(y => y.Id == x.MemberId)! as IMemberModel));
                }
                return _mealPlanMembers;
            }
        }

        [ForeignKey("MealPlanId")]
        public virtual List<Room> Rooms { get; set; } = new();

        [ForeignKey("MealPlanId")]
        public virtual List<MealPlanMember> Members { get; set; } = new();

        public static MealPlan Create(HotelDataBase context, MealPlanBindingModel model)
        {
            return new MealPlan()
            {
                Id = model.Id,
                MealPlanName = model.MealPlanName,
                MealPlanPrice = model.MealPlanPrice,
                OrganiserId = model.OrganiserId,
                Members = model.MealPlanMembers.Select(x => new MealPlanMember
                {
                    Member = context.Members.First(y => y.Id == x.Key),
                }).ToList()
            };
        }

        public void Update(MealPlanBindingModel model)
        {
            MealPlanName = model.MealPlanName;
            MealPlanPrice = model.MealPlanPrice;
            OrganiserId = model.OrganiserId;
        }

        public MealPlanViewModel GetViewModel => new()
        {
            Id = Id,
            MealPlanName = MealPlanName,
            MealPlanPrice = MealPlanPrice,
            OrganiserId=OrganiserId,
            MealPlanMembers = MealPlanMembers
        };

        public void UpdateMembers(HotelDataBase context, MealPlanBindingModel model)
        {
            var mealPlanMembers = context.MealPlanMembers.Where(rec => rec.MealPlanId == model.Id).ToList();

            if (mealPlanMembers != null && mealPlanMembers.Count > 0)
            {
                context.MealPlanMembers.RemoveRange(mealPlanMembers.Where(rec => !model.MealPlanMembers.ContainsKey(rec.MemberId)));
                context.SaveChanges();

                foreach (var updateMember in mealPlanMembers)
                {
                    model.MealPlanMembers.Remove(updateMember.MemberId);
                }
                context.SaveChanges();
            }

            var mealPlan = context.MealPlans.First(x => x.Id == Id);

            foreach (var cm in model.MealPlanMembers)
            {
                context.MealPlanMembers.Add(new MealPlanMember
                {
                    MealPlan = mealPlan,
                    Member = context.Members.First(x => x.Id == cm.Key)
                });
                context.SaveChanges();
            }
            _mealPlanMembers = null;
        }
    }
}
