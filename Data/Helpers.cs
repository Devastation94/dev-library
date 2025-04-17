﻿using System.Text.RegularExpressions;

namespace dev_library.Data
{
    public class Helpers
    {
        public static string GetDifficulty(string difficulty)
        {
            switch (difficulty.ToUpper())
            {
                case "RAID-MYTHIC":
                    return "Mythic Raid";
                case "RAID-HEROIC":
                    return "Heroic Raid";
                case "DUNGEON-MYTHIC10":
                    return "Dungeon";
                case "DUNGEON-MYTHIC-WEEKLY10":
                    return "Dungeon Vault";
                default:
                    return string.Empty;
            }
        }

        public static string GetItemSlot(string itemSlot)
        {
            switch (itemSlot.ToUpper())
            {
                case "FINGER1":
                    return "Ring1";
                case "FINGER2":
                    return "Ring2";
                case "MAIN_HAND":
                    return "Weapon";
                case "OFF_HAND":
                    return "Offhand";
                case "MISCELLANEOUS":
                    return "Curio";
                default:
                    return itemSlot;
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
