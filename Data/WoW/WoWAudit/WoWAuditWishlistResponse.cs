using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_library.Data.WoW.WoWAudit
{
    public class WoWAuditWishlistResponse
    {
        [JsonProperty("created")]
        public string Created { get; set; }
    }
}
