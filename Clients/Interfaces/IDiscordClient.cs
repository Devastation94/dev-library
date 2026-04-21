namespace dev_refined.Clients
{
    public interface IDiscordClient
    {
        Task PostToChannel(ulong channelId, string message);
    }
}
