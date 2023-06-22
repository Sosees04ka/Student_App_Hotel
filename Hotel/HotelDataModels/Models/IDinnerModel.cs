namespace HotelDataModels.Models
{
    public interface IDinnerModel : IId
    {
        string DinnerName { get; }
        double DinnerPrice { get; }
        int HeadwaiterId { get; }
    }
}
