using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.StoragesContracts
{
    public interface IConferenceBookingStorage
    {
        List<ConferenceBookingViewModel> GetFullList();

        List<ConferenceBookingViewModel> GetFilteredList(ConferenceBookingSearchModel model);

        ConferenceBookingViewModel? GetElement(ConferenceBookingSearchModel model);

        ConferenceBookingViewModel? Insert(ConferenceBookingBindingModel model);

        ConferenceBookingViewModel? Update(ConferenceBookingBindingModel model);

        ConferenceBookingViewModel? Delete(ConferenceBookingBindingModel model);
    }
}
