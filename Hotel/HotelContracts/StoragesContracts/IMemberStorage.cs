using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.StoragesContracts
{
    public interface IMemberStorage
    {
        List<MemberViewModel> GetFullList();

        List<MemberViewModel> GetFilteredList(MemberSearchModel model);

        MemberViewModel? GetElement(MemberSearchModel model);

        MemberViewModel? Insert(MemberBindingModel model);

        MemberViewModel? Update(MemberBindingModel model);

        MemberViewModel? Delete(MemberBindingModel model);
    }
}
