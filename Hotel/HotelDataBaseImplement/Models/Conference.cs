using HotelContracts.BindingModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelDataBaseImplement.Models
{
    public class Conference : IConferenceModel
    {
        [Required]
        public string ConferenceName { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        public int OrganiserId { get; private set; }

        public int Id { get; private set; }

        public virtual Organiser Organiser { get; set; }

        private Dictionary<int, IMemberModel> _conferenceMembers = null;

        [NotMapped]
        public Dictionary<int, IMemberModel> ConferenceMembers
        {
            get
            {
                if (_conferenceMembers == null)
                {
                    using var context = new HotelDataBase();
                    _conferenceMembers = Members
                        .ToDictionary(x => x.MemberId, x => (context.Members
                        .FirstOrDefault(y => y.Id == x.MemberId)! as IMemberModel));
                }
                return _conferenceMembers;
            }
        }

        [ForeignKey("ConferenceId")]
        public virtual List<ConferenceBooking> ConferenceBookings { get; set; } = new();

        [ForeignKey("ConferenceId")]
        public virtual List<ConferenceMember> Members { get; set; } = new();

        public static Conference Create(HotelDataBase context, ConferenceBindingModel model)
        {
            return new Conference()
            {
                Id = model.Id,
                ConferenceName = model.ConferenceName,
                StartDate = model.StartDate,
                OrganiserId=model.OrganiserId,
                Members = model.ConferenceMembers.Select(x => new ConferenceMember
                {
                    Member = context.Members.First(y => y.Id == x.Key),
                }).ToList()
            };
        }

        public void Update(ConferenceBindingModel model)
        {
            ConferenceName = model.ConferenceName;
            StartDate = model.StartDate;
            OrganiserId = model.OrganiserId;
        }

        public ConferenceViewModel GetViewModel => new()
        {
            Id = Id,
            ConferenceName = ConferenceName,
            StartDate = StartDate,
            OrganiserId = OrganiserId,
            ConferenceMembers = ConferenceMembers
        };

        public void UpdateMembers(HotelDataBase context, ConferenceBindingModel model)
        {
            var conferenceMembers = context.ConferenceMembers.Where(rec => rec.ConferenceId == model.Id).ToList();

            if (conferenceMembers != null && conferenceMembers.Count > 0)
            {
                context.ConferenceMembers.RemoveRange(conferenceMembers.Where(rec => !model.ConferenceMembers.ContainsKey(rec.MemberId)));
                context.SaveChanges();

                foreach (var updateMember in conferenceMembers)
                {
                    model.ConferenceMembers.Remove(updateMember.MemberId);
                }
                context.SaveChanges();
            }

            var conference = context.Conferences.First(x => x.Id == Id);
            foreach (var cm in model.ConferenceMembers)
            {
                context.ConferenceMembers.Add(new ConferenceMember
                {
                    Conference = conference,
                    Member = context.Members.First(x => x.Id == cm.Key),
                });
                context.SaveChanges();
            }
            _conferenceMembers = null;
        }
    }
}
