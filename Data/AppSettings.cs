using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace dev_library.Data
{
    public static class AppSettings
    {
        public static string BotHookUrl { get; set; }
        public static string RaiderHookUrl { get; set; }
        public static string GuildHookUrl { get; set; }
        public static string NerdHookUrl { get; set; }
        public static string BattleNetClientId { get; set; }
        public static string BattleNetClientSecret { get; set; }
        public static string DiscordBotToken { get; set; }
        public static List<WoWAudit> WoWAudit { get; set; }


        public static void Initialize()
        {
            using (var doc = JsonDocument.Parse(File.ReadAllText("..\\..\\..\\..\\appsettings.json")))
            {
                BotHookUrl = doc.RootElement.GetProperty("BOT_HOOK_URL").ToString();
                RaiderHookUrl = doc.RootElement.GetProperty("RAIDER_HOOK_URL").ToString();
                GuildHookUrl = doc.RootElement.GetProperty("GUILD_HOOK_URL").ToString();
                NerdHookUrl = doc.RootElement.GetProperty("NERD_HOOK_URL").ToString();
                BattleNetClientId = doc.RootElement.GetProperty("BATTLE_NET_CLIENT_ID").ToString();
                BattleNetClientSecret = doc.RootElement.GetProperty("BATTLE_NET_CLIENT_SECRET").ToString();
                DiscordBotToken = doc.RootElement.GetProperty("DISCORD_BOT_TOKEN").ToString();
                WoWAudit = JsonConvert.DeserializeObject<List<WoWAudit>>(doc.RootElement.GetProperty("WOWAUDIT").ToString());
            }
        }
    }

    public class WoWAudit
    {
        [JsonProperty("GUILD")]
        public string Guild { get; set; }
        [JsonProperty("CHANNEL_ID")]
        public ulong ChannelId { get; set; }
        [JsonProperty("TOKEN")]
        public string Token { get; set; }

    }
}
