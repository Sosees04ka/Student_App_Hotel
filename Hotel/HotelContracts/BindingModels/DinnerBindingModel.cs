using HotelDataModels.Models;

namespace HotelContracts.BindingModels
{
    public class DinnerBindingModel : IDinnerModel
    {
        public string DinnerName { get; set; } = string.Empty;

        public double DinnerPrice { get; set; }

        public int HeadwaiterId { get; set; }

        public int Id { get; set; }
    }
}
