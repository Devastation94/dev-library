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
                    return "MYTHIC";
                case "RAID-HEROIC":
                    return "HEROIC";
                default:
                    if (difficulty.ToUpper().StartsWith("DUNGEON-MYTHIC"))
                    {
                        return "M+";
                    }

                    return string.Empty;
            }
        }

        public static string GetItemSlot(string itemSlot)
        {
            switch (itemSlot.ToUpper())
            {
                case "FINGER1":
                    return "RING1";
                case "FINGER2":
                    return "RING2";
                case "MAIN_HAND":
                    return "WEAPON";
                case "OFF_HAND":
                    return "OFFHAND";
                case "MISCELLANEOUS":
                    return "CURIO";
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
