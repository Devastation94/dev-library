using Microsoft.Extensions.Configuration;

namespace dev_library.Data
{
    public static class AppSettings
    {
        public static bool DryRun { get; set; }
        public static bool KeyAudit { get; set; }
        public static DiscordSettings Discord { get; set; }
        public static BattleNetSettings BattleNet { get; set; }
        public static WowAuditSettings[] WowAudit { get; set; }
        public static GoogleSheetsSettings GoogleSheet { get; set; }
        public static FitbitSettings FitbitSettings { get; set; }
        public static GptSettings GptSettings { get; set; }
        public static ServerAvailabilitySettings[] ServerAvailability { get; set; }
        public static string BasePath { get; set; } = $"{Path.GetPathRoot(AppContext.BaseDirectory)}Code";

        public static void Initialize()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            DryRun = config.GetValue<bool>("dryRun");
            KeyAudit = config.GetValue<bool>("keyAudit");
            Discord = config.GetSection("discord").Get<DiscordSettings>();
            BattleNet = config.GetSection("battleNet").Get<BattleNetSettings>();
            WowAudit = config.GetSection("wowAudit").Get<WowAuditSettings[]>();
            GoogleSheet = config.GetSection("googleSheet").Get<GoogleSheetsSettings>();
            FitbitSettings = config.GetSection("fitbit").Get<FitbitSettings>();
            GptSettings = config.GetSection("gpt").Get<GptSettings>();
            ServerAvailability = config.GetSection("serverAvailability").Get<ServerAvailabilitySettings[]>();
        }
    }

    public class GoogleSheetsSettings
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string SheetName { get; set; }
        public string CredentialsPath { get; set; }
    }

    public class WowAuditSettings
    {
        public string Guild { get; set; }
        public ulong[] ChannelIds { get; set; }
        public string Token { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool ReminderOnly { get; set; }
    }

    public class BattleNetSettings
    {
        public string ApiUrl { get; set; }
        public string TokenUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class DiscordSettings
    {
        public Dictionary<string, string> Webhooks { get; set; }
        public string Token { get; set; }
        public ulong UserId { get; set; }
    }

    public class FitbitSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string WebHookUrl { get; set; }
        public string AuthorizationCode { get; set; }
    }

    public class GptSettings
    {
        public string ApiToken { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string AllowedRoles { get; set; }
    }

    public class ServerAvailabilitySettings
    {
        public string Webhook { get; set; }
        public string[] RolesToPing { get; set; }
    }
}
