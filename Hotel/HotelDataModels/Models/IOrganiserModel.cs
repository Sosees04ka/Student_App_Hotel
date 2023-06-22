namespace HotelDataModels.Models
{
    public interface IOrganiserModel : IId
    {
        string OrganiserFIO { get; }
        string OrganiserPassword { get; }
        string OrganiserLogin { get; }
        string OrganiserEmail { get; }
        string OrganiserNumber { get; }
    }
}
