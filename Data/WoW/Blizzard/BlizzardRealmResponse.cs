// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

public class Auctions
{
    [JsonProperty("href")]
    public string Href { get; set; }
}

public class ConnectedRealm
{
    [JsonProperty("href")]
    public string Href { get; set; }
}

public class Key
{
    [JsonProperty("href")]
    public string Href { get; set; }
}

public class Links
{
    [JsonProperty("self")]
    public Self Self { get; set; }
}

public class MythicLeaderboards
{
    [JsonProperty("href")]
    public string Href { get; set; }
}

public class Population
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class Realm
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("region")]
    public Region Region { get; set; }

    [JsonProperty("connected_realm")]
    public ConnectedRealm ConnectedRealm { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("category")]
    public string Category { get; set; }

    [JsonProperty("locale")]
    public string Locale { get; set; }

    [JsonProperty("timezone")]
    public string Timezone { get; set; }

    [JsonProperty("type")]
    public Type Type { get; set; }

    [JsonProperty("is_tournament")]
    public bool IsTournament { get; set; }

    [JsonProperty("slug")]
    public string Slug { get; set; }
}

public class Region
{
    [JsonProperty("key")]
    public Key Key { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }
}

public class BlizzardRealmResponse
{
    [JsonProperty("_links")]
    public Links Links { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("has_queue")]
    public bool HasQueue { get; set; }

    [JsonProperty("status")]
    public Status Status { get; set; }

    [JsonProperty("population")]
    public Population Population { get; set; }

    [JsonProperty("realms")]
    public List<Realm> Realms { get; set; }

    [JsonProperty("mythic_leaderboards")]
    public MythicLeaderboards MythicLeaderboards { get; set; }

    [JsonProperty("auctions")]
    public Auctions Auctions { get; set; }
}

public class Self
{
    [JsonProperty("href")]
    public string Href { get; set; }
}

public class Status
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public class Type
{
    [JsonProperty("type")]
    public string Types { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

