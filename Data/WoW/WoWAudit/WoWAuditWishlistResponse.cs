using Newtonsoft.Json;

namespace dev_library.Data.WoW.WoWAudit
{
    public class WoWAuditWishlistResponse
    {
        [JsonProperty("created")]
        public string Created { get; set; }
        [JsonProperty("base")]
        public string[] Base { get; set; }
    }
}
