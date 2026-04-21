using dev_library.Data;
using dev_refined.Clients;
using Newtonsoft.Json;
using Serilog;

namespace dev_refined
{
    public class RealmClient
    {
        private readonly IDiscordClient _discordClient;
        private readonly IBattleNetClient _battleNetClient;

        public RealmClient() : this(new DiscordClient(), new BattleNetClient()) { }

        public RealmClient(IDiscordClient discordClient, IBattleNetClient battleNetClient)
        {
            _discordClient = discordClient;
            _battleNetClient = battleNetClient;
        }

        public async Task<bool> PostServerAvailability()
        {
            Log.Information("RealmClient.PostServerAvailability: START");
            var fileLocation = $"{AppSettings.BasePath}/realmcache.json";

            var realmData = await _battleNetClient.GetZuljinData();
            var cachedData = JsonConvert.DeserializeObject<BlizzardRealmResponse>(File.ReadAllText(fileLocation));

            File.WriteAllText(fileLocation, JsonConvert.SerializeObject(realmData));

            if (realmData.Status.Name.ToUpper() != cachedData.Status.Name.ToUpper())
            {
                Console.WriteLine($"Server status has changed from {cachedData.Status.Name} to {realmData.Status.Name}");

                if (realmData.Status.Name.ToUpper() == "UP")
                {
                    foreach (var guild in AppSettings.Guilds.Where(g => g.Features.ServerAvailability))
                        await _discordClient.PostToChannel(guild.Channels.GetValueOrDefault("general"), $"Servers are back online! maybe? :3 <@&{string.Join("><@&", guild.RolesToPing)}>");

                    return true;
                }
                else
                {
                    foreach (var guild in AppSettings.Guilds.Where(g => g.Features.ServerAvailability))
                        await _discordClient.PostToChannel(guild.Channels.GetValueOrDefault("general"), "Servers have gone offline! maybe? :3");
                }
            }

            Log.Information("RealmClient.PostServerAvailability: END");
            return false;
        }

    }
}
