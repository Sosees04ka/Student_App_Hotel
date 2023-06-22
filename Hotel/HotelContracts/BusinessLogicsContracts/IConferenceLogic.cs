using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IConferenceLogic
    {
        List<ConferenceViewModel>? ReadList(ConferenceSearchModel? model);
        ConferenceViewModel? ReadElement(ConferenceSearchModel model);
        bool AddMemberToConference(ConferenceSearchModel model, IMemberModel member);
        bool Create(ConferenceBindingModel model);
        bool Update(ConferenceBindingModel model);
        bool Delete(ConferenceBindingModel model);
    }
}
