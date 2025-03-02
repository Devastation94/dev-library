using Newtonsoft.Json;
using System.Text.Json;

namespace dev_library.Data
{
    public static class AppSettings
    {
        public static DiscordSettings Discord { get; set; }
        public static BattleNetSettings BattleNet { get; set; }
        public static WowAuditSettings[] WowAudit { get; set; }
        public static GoogleSheetsSettings GoogleSheet { get; set; }
        public static string BasePath { get; set; } = $"{Path.GetPathRoot(AppContext.BaseDirectory)}Code";

        public static void Initialize()
        {
            var json = File.ReadAllText($"{BasePath}/appsettings.json");
            var config = JsonConvert.DeserializeObject<ConfigData>(json);

            Discord = config.Discord;
            BattleNet = config.BattleNet;
            WowAudit = config.WowAudit;
            GoogleSheet = config.GoogleSheet;
        }

        private class ConfigData
        {
            [JsonProperty("discord")]
            public DiscordSettings Discord { get; set; }
            [JsonProperty("battleNet")]
            public BattleNetSettings BattleNet { get; set; }
            [JsonProperty("wowAudit")]
            public WowAuditSettings[] WowAudit { get; set; }
            [JsonProperty("googleSheet")]
            public GoogleSheetsSettings GoogleSheet { get; set; }
        }
    }

    public class GoogleSheetsSettings
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("sheetName")]
        public string SheetName { get; set; }
        [JsonProperty("credentialsPath")]
        public string CredentialsPath { get; set; }
    }

    public class WowAuditSettings
    {
        [JsonProperty("guild")]
        public string Guild { get; set; }
        [JsonProperty("channelId")]
        public ulong ChannelId { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public class BattleNetSettings
    {
        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }
        [JsonProperty("tokenUrl")]
        public string TokenUrl { get; set; }
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
        [JsonProperty("clientSecret")]
        public string ClientSecret { get; set; }
    }

    public class DiscordSettings
    {
        [JsonProperty("webhooks")]
        public Dictionary<string, string> Webhooks { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("userId")]
        public ulong UserId { get; set; }
    }
}
