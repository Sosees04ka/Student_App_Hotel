using HotelContracts.BindingModels;
using HotelContracts.SearchModels;
using HotelContracts.ViewModels;
using HotelDataModels.Models;

namespace HotelContracts.BusinessLogicsContracts
{
    public interface IRoomLogic
    {
        List<RoomViewModel>? ReadList(RoomSearchModel? model);
        RoomViewModel? ReadElement(RoomSearchModel model);
        bool AddDinnerToRoom(RoomSearchModel model, IDinnerModel dinner);
        bool Create(RoomBindingModel model);
        bool Update(RoomBindingModel model);
        bool Delete(RoomBindingModel model);
    }
}
