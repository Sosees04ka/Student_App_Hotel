using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IHeadwaiterLogic
    {
        List<HeadwaiterViewModel>? ReadList(HeadwaiterSearchModel? model);
        HeadwaiterViewModel? ReadElement(HeadwaiterSearchModel model);
        bool Create(HeadwaiterBindingModel model);
        bool Update(HeadwaiterBindingModel model);
        bool Delete(HeadwaiterBindingModel model);
    }
}
