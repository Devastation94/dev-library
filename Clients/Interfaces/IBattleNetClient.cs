namespace dev_refined.Clients
{
    public interface IBattleNetClient
    {
        Task<BlizzardRealmResponse> GetZuljinData();
    }
}
