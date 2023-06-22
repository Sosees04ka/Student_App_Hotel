using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.StoragesContracts
{
    public interface IOrganiserStorage
    {
        List<OrganiserViewModel> GetFullList();

        List<OrganiserViewModel> GetFilteredList(OrganiserSearchModel model);

        OrganiserViewModel? GetElement(OrganiserSearchModel model);

        OrganiserViewModel? Insert(OrganiserBindingModel model);

        OrganiserViewModel? Update(OrganiserBindingModel model);

        OrganiserViewModel? Delete(OrganiserBindingModel model);
    }
}
