using Newtonsoft.Json;

namespace dev_refined.Data
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Affix
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("wowhead_url")]
        public string WowheadUrl { get; set; }
    }

    public class MythicPlusWeeklyHighestLevelRun
    {
        [JsonProperty("dungeon")]
        public string Dungeon { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("mythic_level")]
        public int MythicLevel { get; set; }

        [JsonProperty("completed_at")]
        public DateTime CompletedAt { get; set; }

        [JsonProperty("clear_time_ms")]
        public int ClearTimeMs { get; set; }

        [JsonProperty("par_time_ms")]
        public int ParTimeMs { get; set; }

        [JsonProperty("num_keystone_upgrades")]
        public int NumKeystoneUpgrades { get; set; }

        [JsonProperty("map_challenge_mode_id")]
        public int MapChallengeModeId { get; set; }

        [JsonProperty("zone_id")]
        public int ZoneId { get; set; }

        [JsonProperty("score")]
        public decimal Score { get; set; }

        [JsonProperty("affixes")]
        public List<Affix> Affixes { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class RaiderIoKeyResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("race")]
        public string Race { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("active_spec_name")]
        public string ActiveSpecName { get; set; }

        [JsonProperty("active_spec_role")]
        public string ActiveSpecRole { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("faction")]
        public string Faction { get; set; }

        [JsonProperty("achievement_points")]
        public int AchievementPoints { get; set; }

        [JsonProperty("honorable_kills")]
        public int HonorableKills { get; set; }

        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("realm")]
        public string Realm { get; set; }

        [JsonProperty("last_crawled_at")]
        public DateTime LastCrawledAt { get; set; }

        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }

        [JsonProperty("profile_banner")]
        public string ProfileBanner { get; set; }

        [JsonProperty("mythic_plus_weekly_highest_level_runs")]
        public List<MythicPlusWeeklyHighestLevelRun> MythicPlusWeeklyHighestLevelRuns { get; set; }

        [JsonProperty("gear")]
        public Gear Gear { get; set; }
    }

    public class Gear
    {
        [JsonProperty("item_level_equipped")]
        public decimal ItemLevel { get; set; }
    }
}
