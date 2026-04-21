using dev_library.Data.WoW.WoWAudit;
using dev_refined.Data;

namespace dev_refined.Clients
{
    public interface IWoWAuditClient
    {
        Task<List<WoWAuditCharacter>> GetCharacters(string guild);
        Task<WoWAuditWishlistResponse> UpdateWishlist(string reportId, string guild);
    }
}
