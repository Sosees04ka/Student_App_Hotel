using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;

namespace HotelContracts.StoragesContracts
{
    public interface IRoomStorage
    {
        List<RoomViewModel> GetFullList();

        List<RoomViewModel> GetFilteredList(RoomSearchModel model);

        RoomViewModel? GetElement(RoomSearchModel model);

        RoomViewModel? Insert(RoomBindingModel model);

        RoomViewModel? Update(RoomBindingModel model);

        RoomViewModel? Delete(RoomBindingModel model);
    }
}
