using System.Text.RegularExpressions;

namespace dev_library.Data
{
    public class Helpers
    {
        public static string GetDifficulty(string difficulty)
        {
            switch (difficulty.ToUpper())
            {
                case "RAID-MYTHIC":
                    return "mythic";
                case "RAID-HEROIC":
                    return "heroic";
                case "DUNGEON-MYTHIC10":
                    return "m+";
                default:
                    return string.Empty;
            }
        }

        public static string GetItemSlot(string itemSlot)
        {
            switch (itemSlot.ToUpper())
            {
                case "TRINKET1":
                case "TRINKET2":
                    return "TRINKET";
                case "FINGER1":
                case "FINGER2":
                    return "RING";
                case "MAIN_HAND":
                    return "1HANDER";
                case "OFF_HAND":
                    return "OFFHAND";
                default:
                    return itemSlot.ToUpper();
            }
        }


        public static List<string> ExtractUrls(string text)
        {
            var pattern = @"https:\/\/(www\.raidbots\.com\/simbot\/report|questionablyepic\.com\/live\/upgradereport)[^\s]*";
            var matches = Regex.Matches(text, pattern);

            var urls = new List<string>();
            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }

            return urls;
        }
    }

}
