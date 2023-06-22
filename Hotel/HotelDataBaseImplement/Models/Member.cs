using HotelContracts.BindingModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelDataBaseImplement.Models
{
    public class Member : IMemberModel
    {
        [Required]
        public string MemberFIO { get; set; } = string.Empty;
        [Required]
        public string Citizenship { get; set; } = string.Empty;

        public int OrganiserId { get; private set; }

        public int Id { get; private set; }

        public virtual Organiser Organiser { get; set; }

        [ForeignKey("MemberId")]
        public virtual List<MealPlanMember> MealPlanMember { get; set; } = new();


        [ForeignKey("MemberId")]
        public virtual List<ConferenceMember> ConferenceMembers { get; set; } = new();

        public static Member? Create(MemberBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Member()
            {
                Id = model.Id,
                MemberFIO = model.MemberFIO,
                Citizenship = model.Citizenship,
                OrganiserId = model.OrganiserId,
            };
        }
        public static Member Create(MemberViewModel model)
        {
            return new Member
            {
                Id = model.Id,
                MemberFIO = model.MemberFIO,
                Citizenship = model.Citizenship,
                OrganiserId=model.OrganiserId,
            };
        }
        public void Update(MemberBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            MemberFIO = model.MemberFIO;
            Citizenship = model.Citizenship;
            OrganiserId = model.OrganiserId;
        }
        public MemberViewModel GetViewModel => new()
        {
            Id = Id,
            MemberFIO = MemberFIO,
            Citizenship = Citizenship,
            OrganiserId=OrganiserId
        };
    }
}
