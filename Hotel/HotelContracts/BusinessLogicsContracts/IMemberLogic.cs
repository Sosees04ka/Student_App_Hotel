using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IMemberLogic
    {
        List<MemberViewModel>? ReadList(MemberSearchModel? model);
        MemberViewModel? ReadElement(MemberSearchModel model);
        bool Create(MemberBindingModel model);
        bool Update(MemberBindingModel model);
        bool Delete(MemberBindingModel model);
    }
}
