namespace HotelDataModels.Models
{
    public interface IMemberModel : IId
    {
        string MemberFIO { get; }
        string Citizenship { get; }
        int OrganiserId { get; }
    }
}
