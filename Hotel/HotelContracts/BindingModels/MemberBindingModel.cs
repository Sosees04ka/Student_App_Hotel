using HotelDataModels.Models;

namespace HotelContracts.BindingModels
{
    public class MemberBindingModel : IMemberModel
    {
        public string MemberFIO { get; set; } = string.Empty;

        public string Citizenship { get; set; } = string.Empty;

        public int OrganiserId { get; set; }

        public int Id { get; set; }
    }
}
