namespace dev_refined.Data
{
    public class BadPlayer
    {
        public BadPlayer(string name, int tensCompleted, string itemLevel)
        {
            Name = name;
            KeysLeft = tensCompleted.ToString();
            ILvl = itemLevel;
        }

        public string Name { get; set; }
        public string KeysLeft { get; set; }
        public string ILvl { get; set; }
    }
}
