using dev_library.Data;
using dev_library.Data.WoW.Blizzard;
using dev_library.Data.WoW.Raidbots;
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
        private const string MAX_HERO_ILVL = "665";
        private static Stopwatch Stopwatch = new Stopwatch();
        private const string cacheName = "wowcache.json";
        public async Task<bool> IsValidReport(string url)
        {
            var bnetClient = new BattleNetClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // TLS 1.3 is not directly supported in .NET Framework

            var handler = new SocketsHttpHandler
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    EnabledSslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
                }
            };

            var content = string.Empty;

            Stopwatch.Restart();

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

            Stopwatch.Stop();
            Console.WriteLine($"IsValidReport took {Stopwatch.ElapsedMilliseconds / 1000}");

            if (content.ToUpper().Contains("HERO 6/6"))
            {
                return true;
            }

            return false;
        }

        public async Task<List<ItemUpgrade>> GetItemUpgrades(string reportId)
        {
            var bnetClient = new BattleNetClient();
            var items = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText($"{AppSettings.BasePath}/{cacheName}"));

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
            Stopwatch.Restart();

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
            Stopwatch.Stop();
            Console.WriteLine($"Getting csv took {Stopwatch.ElapsedMilliseconds / 1000}");

            Stopwatch.Restart();

            content = content.Replace('\t', ',');

            var rows = content.Split('\n');
            var playerRow = rows[1].Split(new char[] { '/', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var playerName = playerRow[0];
            var baseDps = double.Parse(playerRow[1]);

            var itemUpgrades = new List<ItemUpgrade>();
            var token = await bnetClient.GetOAuthToken();

            for (int i = 2; i < rows.Length - 2; i++)
            {
                var parts = rows[i].Split(new char[] { '/', ' ', ',' });
                var difficulty = Helpers.GetDifficulty(parts[2]);
                var dpsGain = Math.Round(double.Parse(parts[9]) - baseDps, 0);
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

                itemUpgrades.Add(new ItemUpgrade(playerName, slot, difficulty, itemName, trueDpsGain));
            }


            Stopwatch.Stop();
            Console.WriteLine($"Converting csv to C# object took {Stopwatch.ElapsedMilliseconds / 1000}");

            return itemUpgrades
               .GroupBy(i => new { i.PlayerName, i.ItemId })  
               .Select(g => g.OrderByDescending(i => i.DpsGain).First())
               .OrderByDescending(i => i.DpsGain) 
               .ToList();
        }
    }
}
