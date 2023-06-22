
namespace HotelDataModels.Models
{
    public interface IHeadwaiterModel : IId
    {
        string HeadwaiterFIO { get; }
        string HeadwaiterPassword { get; }
        string HeadwaiterLogin { get; }
        string HeadwaiterEmail { get; }
        string HeadwaiterNumber { get; }
    }
}
