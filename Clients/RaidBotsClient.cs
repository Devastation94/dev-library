using dev_library.Data.WoW.Raidbots;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace dev_library.Clients
{
    public class RaidBotsClient
    {
        private static string RaidBotsUrl = "https://raidbots.com/reports/";

        public async Task<List<ItemUpgrade>> GetItemUpgrades(string reportId)
        {
            string content = File.ReadAllText("D:/raidbots.csv");

            //using (var httpClient = new HttpClient())
            //{
            //    try
            //    {
            //        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            //        var response = await httpClient.GetAsync("https://www.raidbots.com/reports/oaADZMJUpRAYK8FCA851Mx/data.csv");

            //        if (response.IsSuccessStatusCode)
            //        {
            //            content = await response.Content.ReadAsStringAsync();
            //            Console.WriteLine(content);
            //        }
            //        else
            //        {
            //            Console.WriteLine($"Error: {response.StatusCode}");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Exception: {ex.Message}");
            //    }
            //}

            content = content.Replace('\t', ',');

            var rows = content.Split('\n');
            var playerRow = rows[1].Split(new char[] { '/', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var playerName = playerRow[0];
            var baseDps = double.Parse(playerRow[1]);

            var items = new List<ItemUpgrade>();

            for (int i = 2; i < rows.Length - 2; i++)
            {
                items.Add(new ItemUpgrade(playerName, baseDps, rows[i]));
            }

            var itemUpgrades = items.Where(iu => iu.DpsGain > 0).ToList();

            return itemUpgrades;
        }

        bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // Bypass SSL check
        }
    }
}
