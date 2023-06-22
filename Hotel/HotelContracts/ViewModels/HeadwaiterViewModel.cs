using HotelDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelContracts.ViewModels
{
    public class HeadwaiterViewModel : IHeadwaiterModel
    {
        public int Id { get; set; }
        
        [DisplayName("ФИО метрдотеля")]
        public string HeadwaiterFIO { get; set; } = string.Empty;

        [DisplayName("Пароль метрдотеля")]
        public string HeadwaiterPassword { get; set; } = string.Empty;

        [DisplayName("Логин метрдотеля")]
        public string HeadwaiterLogin { get; set; } = string.Empty;

        [DisplayName("Mail метрдотеля")]
        public string HeadwaiterEmail { get; set; } = string.Empty;

        [DisplayName("Телефон метрдотеля")]
        public string HeadwaiterNumber { get; set; } = string.Empty;

    }
}
