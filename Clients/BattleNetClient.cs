using dev_library.Data;
using dev_refined.Data.Realms;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace dev_refined.Clients
{
    public class BattleNetClient
    {
        const string battleNetApiUrl = "https://us.api.blizzard.com/data/wow/connected-realm/61?namespace=dynamic-us&locale=en_US&access_token=";
        const string battleNetTokenUrl = "https://oauth.battle.net/token";

        public async Task<string> GetOAuthToken()
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("POST"), battleNetTokenUrl);

            var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{AppSettings.BattleNetClientId}:{AppSettings.BattleNetClientSecret}"));
            request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

            request.Content = new StringContent("grant_type=client_credentials");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            return JsonConvert.DeserializeObject<BlizzardOAuthResponse>(response).AccessToken;
        }

        public async Task<BlizzardRealmResponse> GetServerInformation(string token)
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), battleNetApiUrl);
            request.Headers.TryAddWithoutValidation("accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false); ;

            return JsonConvert.DeserializeObject<BlizzardRealmResponse>(response);
        }
    }
}