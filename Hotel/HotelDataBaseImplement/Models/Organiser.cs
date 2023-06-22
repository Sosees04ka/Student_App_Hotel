using HotelContracts.BindingModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelDataBaseImplement.Models
{
    public class Organiser : IOrganiserModel
    {
        [Required]
        public string OrganiserFIO { get; set; } = string.Empty;
        [Required]
        public string OrganiserPassword { get; set; } = string.Empty;
        [Required]
        public string OrganiserLogin { get; set; } = string.Empty;
        [Required]
        public string OrganiserEmail { get; set; } = string.Empty;
        [Required]
        public string OrganiserNumber { get; set; } = string.Empty;

        public int Id { get; private set; }

        [ForeignKey("OrganiserId")]
        public virtual List<Conference> Conferences { get; set; } = new();
        [ForeignKey("OrganiserId")]
        public virtual List<MealPlan> MealPlans { get; set; } = new();
        [ForeignKey("OrganiserId")]
        public virtual List<Member> Members { get; set; } = new();

        public static Organiser? Create(OrganiserBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Organiser()
            {
                Id = model.Id,
                OrganiserFIO = model.OrganiserFIO,
                OrganiserEmail = model.OrganiserEmail,
                OrganiserPassword = model.OrganiserPassword,
                OrganiserLogin = model.OrganiserLogin,
                OrganiserNumber = model.OrganiserNumber
            };
        }
        public static Organiser Create(OrganiserViewModel model)
        {
            return new Organiser
            {
                Id = model.Id,
                OrganiserFIO = model.OrganiserFIO,
                OrganiserEmail = model.OrganiserEmail,
                OrganiserPassword = model.OrganiserPassword,
                OrganiserLogin= model.OrganiserLogin,
                OrganiserNumber= model.OrganiserNumber
            };
        }
        public void Update(OrganiserBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            OrganiserFIO = model.OrganiserFIO;
            OrganiserEmail = model.OrganiserEmail;
            OrganiserPassword = model.OrganiserPassword;
            OrganiserLogin = model.OrganiserLogin;
            OrganiserNumber = model.OrganiserNumber;
        }
        public OrganiserViewModel GetViewModel => new()
        {
            Id = Id,
            OrganiserFIO = OrganiserFIO,
            OrganiserEmail = OrganiserEmail,
            OrganiserPassword = OrganiserPassword,
            OrganiserNumber = OrganiserNumber,
            OrganiserLogin = OrganiserLogin
        };
    }
}
