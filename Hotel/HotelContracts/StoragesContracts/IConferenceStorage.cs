using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.StoragesContracts
{
    public interface IConferenceStorage
    {
        List<ConferenceViewModel> GetFullList();

        List<ConferenceViewModel> GetFilteredList(ConferenceSearchModel model);

        ConferenceViewModel? GetElement(ConferenceSearchModel model);

        ConferenceViewModel? Insert(ConferenceBindingModel model);

        ConferenceViewModel? Update(ConferenceBindingModel model);

        ConferenceViewModel? Delete(ConferenceBindingModel model);
    }
}
