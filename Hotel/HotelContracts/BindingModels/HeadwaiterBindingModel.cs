using HotelDataModels.Models;

namespace HotelContracts.BindingModels
{
    public class HeadwaiterBindingModel : IHeadwaiterModel
    {
        public string HeadwaiterFIO { get; set; } = string.Empty;

        public string HeadwaiterPassword { get; set; } = string.Empty;

        public string HeadwaiterLogin { get; set; } = string.Empty;

        public string HeadwaiterEmail { get; set; } = string.Empty;

        public string HeadwaiterNumber { get; set; } = string.Empty;

        public int Id { get; set; }
    }
}
