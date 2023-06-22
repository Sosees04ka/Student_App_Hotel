using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.StoragesContracts
{
    public interface IDinnerStorage
    {
        List<DinnerViewModel> GetFullList();

        List<DinnerViewModel> GetFilteredList(DinnerSearchModel model);

        DinnerViewModel? GetElement(DinnerSearchModel model);

        DinnerViewModel? Insert(DinnerBindingModel model);

        DinnerViewModel? Update(DinnerBindingModel model);

        DinnerViewModel? Delete(DinnerBindingModel model);
    }
}
