using dev_library.Data;
using dev_library.Data.WoW.WoWAudit;
using dev_refined.Data;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

namespace dev_refined.Clients
{
    public class WoWAuditClient
    {
        public async Task<List<WoWAuditCharacter>> GetCharacters(string guild)
        {
            try
            {
                using var client = new HttpClient();
                using var request = new HttpRequestMessage(new HttpMethod("GET"), $"{Constants.WOW_AUDIT_URL}/characters");

                request.Headers.TryAddWithoutValidation("accept", "application/json");
                request.Headers.TryAddWithoutValidation("Authorization", AppSettings.WowAudit.First(wa => wa.Guild == guild.ToUpper()).Token);

                var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false);

                var guildies = JsonConvert.DeserializeObject<List<WoWAuditCharacter>>(response);

                Log.Information($"Found {guildies.Count}");

                return guildies;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to get wow audit characters");
                throw;
            }
        }
        public async Task<WoWAuditWishlistResponse> UpdateWishlist(string reportId, string guild)
        {
            var response = "";

            Console.WriteLine("WoWAuditClient.UpdateWishlist: START");
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.WowAudit.First(wa => wa.Guild == guild.ToUpper()).Token);
                var requestBody = new StringContent(JsonConvert.SerializeObject(new WoWAuditWishlistRequest(reportId)), Encoding.UTF8, ContentType.Json);
                response = await client.PostAsync($"{Constants.WOW_AUDIT_URL}/wishlists", requestBody).Result.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<WoWAuditWishlistResponse>(response);
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to update wowaudit wishlist");
                Console.WriteLine(response);
                throw;
            }
            finally
            {
                Console.WriteLine("WoWAuditClient.UpdateWishlist: END");
            }
        }
    }
}