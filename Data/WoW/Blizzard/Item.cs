﻿using Newtonsoft.Json;

namespace dev_library.Data.WoW.Blizzard
{
    public class Item
    {
        public Item(string name, string id)
        {
            Name = name;
            Id = id;
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
