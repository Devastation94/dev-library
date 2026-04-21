using dev_refined.Data;

namespace dev_refined.Clients
{
    public interface IRaiderIoClient
    {
        Task<RaiderIoKeyResponse> GetWeeklyKeyHistory(WoWAuditCharacter guildy);
    }
}
