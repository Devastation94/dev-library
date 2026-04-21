using dev_library.Data;
using Serilog;

namespace dev_refined.Clients
{
    public class DiscordClient : IDiscordClient
    {
        public static Func<ulong, string, Task>? SendMessageAsync { get; set; }

        public async Task PostToChannel(ulong channelId, string message)
        {
            Log.Information("DiscordClient.PostToChannel: START");

            if (SendMessageAsync != null)
                await SendMessageAsync(channelId, message);
            else
                Console.WriteLine($"[WARN] DiscordClient.SendMessageAsync not wired up. Message: {message}");

            Log.Information("DiscordClient.PostToChannel: END");
        }

        public async Task PostWebHook(List<Search> searchResults)
        {
            Log.Information("DiscordClient.PostWebHook: START");
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
                    if (SendMessageAsync != null)
                        await SendMessageAsync(AppSettings.Guilds.First(g => g.Name == "POKEMON").Channels.GetValueOrDefault("general"), webHookValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            Log.Information("DiscordClient.PostWebHook: END");
        }
    }
}
