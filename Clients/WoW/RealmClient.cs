using dev_refined.Clients;
using Newtonsoft.Json;
using Serilog;

namespace dev_refined
{
    public class RealmClient
    {

        const string fileLocation = "cache.json";
        DiscordClient discordClient = new DiscordClient();
        BattleNetClient battleNetClient = new BattleNetClient();

        public async Task GetServerAvailibility()
        {
            var realmStatus = false;

            while (!realmStatus)
            {
                using (var client = new HttpClient())
                {
                    var usersToPing = new[] { "217441514161176577", "154306391400513536", "178295063808311297", "285277811348996097" };

                    Log.Information("");

                    var token = await battleNetClient.GetOAuthToken();

                    var realmData = await battleNetClient.GetServerInformation(token);

                    var cachedData = JsonConvert.DeserializeObject<BlizzardRealmResponse>(File.ReadAllText(fileLocation));

                    if (realmData.Status.Name.ToUpper() != cachedData.Status.Name.ToUpper() && realmData.Status.Name.ToUpper() == "UP")
                    {
                        Log.Information($"Server status has changed from {cachedData.Status.Name} to {realmData.Status.Name}");
                        var content = $"Server status has changed from {cachedData.Status.Name} to {realmData.Status.Name} maybe? :3";
                        await discordClient.PostWebHook(content + string.Join(" ", usersToPing.Select(user => $"<@{user}>")));
                        File.WriteAllText(fileLocation, JsonConvert.SerializeObject(realmData));
                    }
                    else
                    {
                        Log.Information($"Server status has not changed from {cachedData.Status.Name}");
                    }

                    realmStatus = realmData.Status.Name.ToUpper() == "UP";
                }
                Thread.Sleep(1000);
            }
            Log.Information("Servers are now up. Ending execution");
        }
    }
}
