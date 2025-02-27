namespace dev_library.Data.WoW.Raidbots
{
    public class ItemUpgrade
    {
        public ItemUpgrade(string player, double baseDps, string csvRow)
        {
            var parts = csvRow.Split(new char[] { '/', ' ', ',' });
            PlayerName = player;
            Difficulty = Helpers.GetDifficulty(parts[2]);
            ItemId = parts[3];
            Slot = Helpers.GetItemSlot(parts[6]);
            DpsGain = Math.Round(double.Parse(parts[9]) - baseDps, 0);
        }

        public ItemUpgrade(string player, string slot, string difficulty, string itemName, double dpsGain)
        {
            PlayerName = player;
            Slot = slot;
            Difficulty = difficulty;
            ItemId = itemName;
            DpsGain = dpsGain;
        }

        public string PlayerName { get; set; }
        public string Slot { get; set; }
        public string Difficulty { get; set; }
        public string ItemId { get; set; }
        public double DpsGain { get; set; }
    }
}
