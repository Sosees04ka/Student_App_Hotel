using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IDinnerLogic
    {
        List<DinnerViewModel>? ReadList(DinnerSearchModel? model);
        DinnerViewModel? ReadElement(DinnerSearchModel model);
        bool Create(DinnerBindingModel model);
        bool Update(DinnerBindingModel model);
        bool Delete(DinnerBindingModel model);
        
    }
}
