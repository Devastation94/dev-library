using dev_refined.Clients;
using dev_refined.Data;
using Serilog;

namespace dev_refined
{
    public class RefinedClient
    {
        WoWAuditClient WowAuditClient = new();
        RaiderIoClient RaiderIoClient = new();
        DiscordClient DiscordClient = new();

        public async Task PostBadPlayers()
        {
            Log.Information("RefinedClient.PostBadPlayers: START");
            var badPlayers = new List<BadPlayer>();
            var paddingArray = new int[] { 14, 10, 6 };

            var guildies = await WowAuditClient.GetCharacters("Refined");

            foreach (var guildy in guildies.Where(g => g.Rank.ToUpper() != "RAIDER ALT"))
            {
                Log.Information($"RefinedClient.PostBadPlayers: Getting key info for {guildy.Name}");

                var weeklyKeys = await RaiderIoClient.GetWeeklyKeyHistory(guildy);
                var maxKeyCount = weeklyKeys?.MythicPlusWeeklyHighestLevelRuns.Count(k => k.MythicLevel >= Constants.MAX_KEY_LEVEL) ?? 0;

                Log.Information($"RefinedClient.PostBadPlayers: {guildy.Name} performed {maxKeyCount} +{Constants.MAX_KEY_LEVEL}s this week");

                // Validates that they have done 8 + 10s this week
                if (maxKeyCount < 8)
                {
                    badPlayers.Add(new BadPlayer(guildy.Name, 8 - maxKeyCount, decimal.Round(weeklyKeys.Gear.ItemLevel).ToString()));
                }
            }

            var table = $"Key Audit for +{Constants.MAX_KEY_LEVEL} keys\n```\n";
            table += $"|--------------|----------|------|\n";

            var props = badPlayers[0].GetType().GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                table += $"|{props[i].Name.PadBoth(paddingArray[i], ' ')}";
            }

            table += "|";

            foreach (var badPlayer in badPlayers.OrderByDescending(bp => bp.KeysLeft))
            {
                table += "\n";

                props = badPlayer.GetType().GetProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    table += $"|{props[i].GetValue(badPlayer).ToString().PadBoth(paddingArray[i], ' ')}";
                }

                table += "|";
            }

            table += $"\n|--------------|----------|------|\n```";

            DiscordClient.PostWebHook(table);
            Log.Information("RefinedClient.PostBadPlayers: END");
        }
    }
}