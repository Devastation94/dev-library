using dev_library.Data;
using dev_library.Data.WoW.Blizzard;
using dev_library.Data.WoW.Raidbots;
using dev_refined;
using dev_refined.Clients;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;

namespace dev_library.Clients
{
    public class RaidBotsClient
    {
        private static string RaidBotsFileUrl = "https://raidbots.com/reports/{0}/data.csv";
        private const string cacheName = "wowcache.json";
        public async Task<bool> IsValidReport(string url)
        {
            Console.WriteLine("RaidBotsClients.IsValidReport: START");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // TLS 1.3 is not directly supported in .NET Framework

            var handler = new SocketsHttpHandler
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
                }
            };

            var content = string.Empty;

            using (var httpClient = new HttpClient(handler))
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        // Console.WriteLine(content);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }

            if (content.ToUpper().Contains("HERO 6/6") || content.ToUpper().Contains("MYTH 6/6"))
            {
                return true;
            }

            Console.WriteLine("RaidBotsClients.IsValidReport: END");
            return false;
        }

        public async Task<List<ItemUpgrade>> GetItemUpgrades(List<ItemUpgrade> itemUpgrades, string reportId)
        {
            Console.WriteLine("RaidBotsClients.GetItemUpgrades: START");

            var bnetClient = new BattleNetClient();
            var items = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText($"{AppSettings.BasePath}/{cacheName}"));

            var lastUpdated = DateTime.Now;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // TLS 1.3 is not directly supported in .NET Framework
            var url = string.Format(RaidBotsFileUrl, reportId);

            var handler = new SocketsHttpHandler
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
                }
            };

            var content = string.Empty;

            using (var httpClient = new HttpClient(handler))
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                    var response = await httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        // Console.WriteLine(content);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }

            content = content.Replace('\t', ',');

            var rows = content.Split('\n');
            var playerRow = rows[1].Split(new char[] { '/', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var playerName = playerRow[0].ToTitleCase();
            var baseDps = double.Parse(playerRow[1]);

            var token = await bnetClient.GetOAuthToken();

            for (int i = 2; i < rows.Length - 2; i++)
            {
                var parts = rows[i].Split(new char[] { '/', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                var difficulty = Helpers.GetDifficulty(parts[2]);
                var dpsGain = Math.Round(double.Parse(parts[^5]) - baseDps, 0);
                var trueDpsGain = difficulty == "M+" ? dpsGain * 1.1 : dpsGain;

                if (dpsGain < 0)
                {
                    continue;
                }

                var itemName = string.Empty;

                var item = items.FirstOrDefault(i => i.Id == parts[3]);

                if (item == null)
                {
                    itemName = await bnetClient.GetItemName(token, parts[3]);
                    items.Add(new Item(itemName, parts[3]));
                }
                else
                {
                    itemName = item.Name;
                }

                File.WriteAllText($"{AppSettings.BasePath}/{cacheName}", JsonConvert.SerializeObject(items));

                var slot = Helpers.GetItemSlot(parts[6]);

                var itemUpgrade = new ItemUpgrade(playerName, slot, difficulty, itemName, trueDpsGain, lastUpdated);

                var existingSlotIndex = itemUpgrades.FindIndex(i => i.Slot.ToUpper().Trim() == slot.ToUpper().Trim());
                var existingItemIndex = itemUpgrades.FindIndex(i => i.ItemName.ToUpper().Trim() == itemName.ToUpper().Trim());

                if (existingItemIndex != -1 && trueDpsGain > itemUpgrades[existingItemIndex].DpsGain)
                {
                    itemUpgrades[existingItemIndex] = itemUpgrade;
                }
                else
                {
                    itemUpgrades.Add(itemUpgrade);
                }
            }

            itemUpgrades = itemUpgrades.OrderByDescending(iu => iu.DpsGain).ToList().GroupBy(sd => new { sd.ItemName, sd.PlayerName, sd.Difficulty }).Select(g => g.First()).ToList();
            itemUpgrades = itemUpgrades.OrderByDescending(iu => iu.DpsGain).ToList().GroupBy(sd => new { sd.Slot, sd.PlayerName, sd.Difficulty }).Select(g => g.First()).ToList();

            //var existingSlotIndex = itemUpgrades.FindIndex(i => i.Slot.ToUpper().Trim() == slot.ToUpper().Trim());
            //var existingItemIndex = itemUpgrades.FindIndex(i => i.ItemName.ToUpper().Trim() == itemName.ToUpper().Trim());

            //if (existingItemIndex == -1 && existingSlotIndex == -1)
            //{
            //    Console.WriteLine($"Adding item {itemName} to upgrades with {trueDpsGain} dps gain");
            //    itemUpgrades.Add(new ItemUpgrade(playerName, slot, difficulty, itemName, trueDpsGain, lastUpdated));
            //}
            //else if (existingSlotIndex != -1 && existingItemIndex != -1 && trueDpsGain > itemUpgrades[existingSlotIndex].DpsGain)
            //{
            //    Console.WriteLine($"Duplicate item found {itemName} changing dps gain to {trueDpsGain}");
            //    itemUpgrades[existingSlotIndex] = new ItemUpgrade(playerName, slot, difficulty, itemName, trueDpsGain, lastUpdated);
            //}

            Console.WriteLine("RaidBotsClients.GetItemUpgrades: END");

            return itemUpgrades;
        }
    }
}
