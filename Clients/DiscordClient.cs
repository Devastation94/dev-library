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

            try
            {
                using var client = new HttpClient();
                var discordBody = JsonConvert.SerializeObject(new DiscordRequest(message));
                var response = await client.PostAsync(AppSettings.Discord.Webhooks["GUILDCHAT"], new StringContent(discordBody, Encoding.UTF8, ContentType.Json));
                var responseContent = response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  
            }

            Log.Information("DiscordClient.PostWebHook: END");
            return;
        }

        public async Task PostWebHook(List<Search> searchResults)
        {
            foreach (var storeGroup in searchResults.GroupBy(sr => sr.Store))
            {
                var webHookValue = $"- {storeGroup.Key}\n";

                foreach (var itemInStock in storeGroup)
                {
                    webHookValue += $"  - {itemInStock.Keyword}\n";

                    foreach (var product in itemInStock.Products)
                    {
                        var productInfo = $"New Item Now In Stock: {product.Name}, Price: {product.Price}";
                        Console.WriteLine($"Program.PostResults: {productInfo}");
                        webHookValue += $"      - {product.Url}";
                    }
                }

                try
                {
                    using var client = new HttpClient();
                    var discordBody = JsonConvert.SerializeObject(new DiscordRequest(webHookValue));
                    var response = await client.PostAsync(AppSettings.Discord.Webhooks["POKEMON"], new StringContent(discordBody, Encoding.UTF8, "application/json"));
                    var responseContent = response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return;
        }
    }
}
