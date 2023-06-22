using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IOrganiserLogic
    {
        List<OrganiserViewModel>? ReadList(OrganiserSearchModel? model);
        OrganiserViewModel? ReadElement(OrganiserSearchModel model);
        bool Create(OrganiserBindingModel model);
        bool Update(OrganiserBindingModel model);
        bool Delete(OrganiserBindingModel model);
    }
}
