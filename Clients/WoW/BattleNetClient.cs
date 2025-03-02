using dev_library.Data;
using dev_refined.Data.Realms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace dev_refined.Clients
{
    public class BattleNetClient
    {
        const string realmDataEndpoint = "/connected-realm/61?namespace=dynamic-us&locale=en_US";
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

        public async Task<BlizzardRealmResponse> GetServerInformation(string token)
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), realmDataEndpoint);
            request.Headers.TryAddWithoutValidation("accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            return JsonConvert.DeserializeObject<BlizzardRealmResponse>(response);
        }

        public async Task<string> GetItemName(string token, string itemId)
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), string.Format(itemNameEndpoint, itemId));
            request.Headers.TryAddWithoutValidation("accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            return JsonConvert.DeserializeObject<JObject>(response)["name"].ToString();
        }
    }
}