using HotelDataModels.Models;
using System.ComponentModel;

namespace HotelContracts.ViewModels
{
    public class OrganiserViewModel : IOrganiserModel
    {
        [DisplayName("ФИО организатора")]
        public string OrganiserFIO { get; set; } = string.Empty;

        [DisplayName("Пароль организатора")]
        public string OrganiserPassword { get; set; } = string.Empty;

        [DisplayName("Логин организатора")]
        public string OrganiserLogin { get; set; } = string.Empty;

        [DisplayName("Эл. почта организатора")]
        public string OrganiserEmail { get; set; } = string.Empty;

        [DisplayName("Номер телефона организатора")]
        public string OrganiserNumber { get; set; } = string.Empty;

        public int Id { get; set; }
    }
}
