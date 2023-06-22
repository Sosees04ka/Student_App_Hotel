using HotelDataModels.Models;
using System.ComponentModel;


namespace HotelContracts.ViewModels
{
    public class DinnerViewModel : IDinnerModel
    {
        public int Id { get; set; }
        public int HeadwaiterId { get; set; }

        [DisplayName("Название обеда")]
        public string DinnerName { get; set; } = string.Empty;

        [DisplayName("Цена обеда")]
        public double DinnerPrice { get; set; }
    }
}

