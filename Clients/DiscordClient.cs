using dev_library.Data;
using dev_refined.Data;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Text;

namespace dev_refined.Clients
{
    public class DiscordClient
    {
        public async Task PostWebHook(string message)
        {
            Log.Information("DiscordClient.PostWebHook: START");
            using var client = new HttpClient();
            var discordBody = JsonConvert.SerializeObject(new DiscordRequest(message));
            var response = await client.PostAsync(AppSettings.NerdHookUrl, new StringContent(discordBody, Encoding.UTF8, ContentType.Json));

            Log.Information("DiscordClient.PostWebHook: END");
            return;
        }
    }
}
