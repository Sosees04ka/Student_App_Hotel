using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.StoragesContracts
{
    public interface IHeadwaiterStorage
    {
        List<HeadwaiterViewModel> GetFullList();

        List<HeadwaiterViewModel> GetFilteredList(HeadwaiterSearchModel model);

        HeadwaiterViewModel? GetElement(HeadwaiterSearchModel model);

        HeadwaiterViewModel? Insert(HeadwaiterBindingModel model);

        HeadwaiterViewModel? Update(HeadwaiterBindingModel model);

        HeadwaiterViewModel? Delete(HeadwaiterBindingModel model);
    }
}
