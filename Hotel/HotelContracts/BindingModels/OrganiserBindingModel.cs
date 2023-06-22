using HotelDataModels.Models;

namespace HotelContracts.BindingModels
{
    public class OrganiserBindingModel : IOrganiserModel
    {
        public string OrganiserFIO { get; set; } = string.Empty;

        public string OrganiserPassword { get; set; } = string.Empty;

        public string OrganiserLogin { get; set; } = string.Empty;

        public string OrganiserEmail { get; set; } = string.Empty;

        public string OrganiserNumber { get; set; } = string.Empty;

        public int Id { get; set; }
    }
}
