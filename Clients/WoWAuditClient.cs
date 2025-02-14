﻿using System.Net.Http.Headers;
using System.Text;
using dev_library.Data;
using dev_library.Data.WoW.WoWAudit;
using dev_refined.Data;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace dev_refined.Clients
{
    public class WoWAuditClient
    {
        public async Task<List<WoWAuditCharacter>> GetCharacters()
        {
            using var client = new HttpClient();
            using var request = new HttpRequestMessage(new HttpMethod("GET"), $"{Constants.WOW_AUDIT_URL}/characters");

            request.Headers.TryAddWithoutValidation("accept", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", AppSettings.WoWAuditToken);

            var response = await client.SendAsync(request).Result.Content.ReadAsStringAsync().ConfigureAwait(false);

            var guildies = JsonConvert.DeserializeObject<List<WoWAuditCharacter>>(response);

            Log.Information($"Found {guildies.Count}");

            return guildies;
        }
        public async Task<bool> UpdateWishlist(string reportId)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.WoWAuditToken);
            var requestBody = new StringContent(JsonConvert.SerializeObject(new WoWAuditWishlistRequest(reportId)), Encoding.UTF8, ContentType.Json);
            var response = await client.PostAsync($"{Constants.WOW_AUDIT_URL}/wishlists", requestBody).Result.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<WoWAuditWishlistResponse>(response).Created == "true";
        }
    }
}