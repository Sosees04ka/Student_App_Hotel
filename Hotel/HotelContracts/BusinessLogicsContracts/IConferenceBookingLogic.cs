using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IConferenceBookingLogic
    {
        List<ConferenceBookingViewModel>? ReadList(ConferenceBookingSearchModel? model);
        ConferenceBookingViewModel? ReadElement(ConferenceBookingSearchModel model);
        bool AddDinnerToConferenceBooking(ConferenceBookingSearchModel model, IDinnerModel dinner);
        bool Create(ConferenceBookingBindingModel model);
        bool Update(ConferenceBookingBindingModel model);
        bool Delete(ConferenceBookingBindingModel model);
    }
}
