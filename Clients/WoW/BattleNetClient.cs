using dev_library.Data;
using dev_library.Data.WoW.Blizzard;
using dev_refined.Data.Realms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;

namespace dev_refined.Clients
{
    public class BattleNetClient
    {
        const string realmDataEndpoint = "/connected-realm/61?namespace=dynamic-us&locale=en_US";
        const string allRealmsEndpoint = "/connected-realm/index?namespace=dynamic-us&locale=en_US";
        const string itemNameEndpoint = "/item/{0}?namespace=static-us&locale=en_US";

        public async Task<string> GetOAuthToken()
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), AppSettings.BattleNet.TokenUrl);

            var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{AppSettings.BattleNet.ClientId}:{AppSettings.BattleNet.ClientSecret}"));
            request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

            request.Content = new StringContent("grant_type=client_credentials");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            return JsonConvert.DeserializeObject<BlizzardOAuthResponse>(response).AccessToken;
        }

        public async Task<BlizzardRealmResponse> GetZuljinData()
        {
            var token = await GetOAuthToken();
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), AppSettings.BattleNet.ApiUrl + realmDataEndpoint);
            request.Headers.TryAddWithoutValidation("accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            return JsonConvert.DeserializeObject<BlizzardRealmResponse>(response);
        }

        public async Task<BlizzardRealmsResponse> GetRealms()
        {
            var token = await GetOAuthToken();
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), AppSettings.BattleNet.ApiUrl + allRealmsEndpoint);
            request.Headers.TryAddWithoutValidation("accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false);

            var realmList = JsonConvert.DeserializeObject<BlizzardRealmsResponse>(response);

            return realmList;
        }

        public async Task<string> GetItemName(string itemId)
        {
            var token = await GetOAuthToken();
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format(AppSettings.BattleNet.ApiUrl + itemNameEndpoint, itemId));
            request.Headers.TryAddWithoutValidation("accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            return JsonConvert.DeserializeObject<JObject>(response)["name"].ToString();
        }

        public async Task GetAuctions(BlizzardRealmsResponse realmData)
        {
            var token = await GetOAuthToken();
            var rings = new List<Auction>();

            foreach (var realm in realmData.ConnectedRealms)
            {
                using var client = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("GET"), realm.Href + "/auctions?namespace=dynamic-us&locale=en_US");
                request.Headers.TryAddWithoutValidation("accept", "application/json");
                request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

                var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false);

                var auctions = JsonConvert.DeserializeObject<BlizzardAuctionResponse>(response);

                rings.AddRange(auctions.Auctions.Where(a => a.Item.Id == "238036" && a.Item.BonusLists.Contains(10355)));

            }


            Console.ReadLine();
            // return JsonConvert.DeserializeObject<>(response)["name"].ToString();
        }
    }
}