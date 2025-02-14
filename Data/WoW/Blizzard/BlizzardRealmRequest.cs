namespace dev_refined.Data.Realms
{
    public class BlizzardRealmRequest
    {
        public string Region { get; set; } = "us";
        public int ConnectionRealmId { get; set; } = 61;
        public string NameSpace { get; set; } = "dynamic_us";
        public string Locale { get; set; } = "en_US";
    }
}
