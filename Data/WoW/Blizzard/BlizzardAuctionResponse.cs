﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_library.Data.WoW.Blizzard
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Auction
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("item")]
        public Item Item;

        [JsonProperty("buyout")]
        public long Buyout;

        [JsonProperty("quantity")]
        public uint Quantity;

        [JsonProperty("time_left")]
        public string TimeLeft;

        [JsonProperty("bid")]
        public long? Bid;
    }

    public class Modifier
    {
        [JsonProperty("type")]
        public int Type;

        [JsonProperty("value")]
        public int Value;

    }

    public class BlizzardAuctionResponse
    {
        [JsonProperty("_links")]
        public Links Links;

        [JsonProperty("connected_realm")]
        public ConnectedRealm ConnectedRealm;

        [JsonProperty("auctions")]
        public List<Auction> Auctions;
    }


}
