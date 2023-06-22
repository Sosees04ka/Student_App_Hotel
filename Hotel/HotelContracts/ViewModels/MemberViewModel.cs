using HotelDataModels.Models;
using System.ComponentModel;

namespace HotelContracts.ViewModels
{
    public class MemberViewModel : IMemberModel
    {
        public int Id { get; set; }

        [DisplayName("ФИО участника")]
        public string MemberFIO { get; set; } = string.Empty;

        [DisplayName("Гражданство")]
        public string Citizenship { get; set; } = string.Empty;

        public int OrganiserId { get; set; }

        
    }
}
