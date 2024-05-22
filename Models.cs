using System.Text.Json.Serialization;

namespace OsuMultiAnalyser;

public class Beatmap
{
    public int beatmapset_id { get; set; }
    public float difficulty_rating { get; set; }
    public int id { get; set; }
    public string mode { get; set; }
    public string status { get; set; }
    public int total_length { get; set; }
    public int user_id { get; set; }
    public string version { get; set; }
    public Beatmapset beatmapset { get; set; }
}

public class Beatmapset
{
    public string artist { get; set; }
    public string artist_unicode { get; set; }
    public Covers covers { get; set; }
    public string creator { get; set; }
    public int favourite_count { get; set; }
    public object hype { get; set; }
    public int id { get; set; }
    public bool nsfw { get; set; }
    public int offset { get; set; }
    public int play_count { get; set; }
    public string preview_url { get; set; }
    public string source { get; set; }
    public bool spotlight { get; set; }
    public string status { get; set; }
    public string title { get; set; }
    public string title_unicode { get; set; }
    public int? track_id { get; set; }
    public int user_id { get; set; }
    public bool video { get; set; }
}

public class Country
{
    public string code { get; set; }
    public string name { get; set; }
}

public class Covers
{
    public string cover { get; set; }

    [JsonPropertyName("cover@2x")]
    public string cover2x { get; set; }
    public string card { get; set; }

    [JsonPropertyName("card@2x")]
    public string card2x { get; set; }
    public string list { get; set; }

    [JsonPropertyName("list@2x")]
    public string list2x { get; set; }
    public string slimcover { get; set; }

    [JsonPropertyName("slimcover@2x")]
    public string slimcover2x { get; set; }
}

public class CurrentUserAttributes
{
    public object pin { get; set; }
}

public class Detail
{
    public string type { get; set; }
    public string text { get; set; }
}

public class Event
{
    public object id { get; set; }
    public Detail detail { get; set; }
    public DateTime timestamp { get; set; }
    public int? user_id { get; set; }
    public Game game { get; set; }
}

public class Game
{
    public int beatmap_id { get; set; }
    public int id { get; set; }
    public DateTime start_time { get; set; }
    public DateTime end_time { get; set; }
    public string mode { get; set; }
    public int mode_int { get; set; }
    public string scoring_type { get; set; }
    public string team_type { get; set; }
    public List<string> mods { get; set; }
    public Beatmap beatmap { get; set; }
    public List<Score> scores { get; set; }
}

public class Match
{
    public int id { get; set; }
    public DateTime start_time { get; set; }
    public DateTime end_time { get; set; }
    public string name { get; set; }
    public int slot { get; set; }
    public string team { get; set; }
    public bool pass { get; set; }
}

public class Root
{
    public Match match { get; set; }
    public List<Event> events { get; set; }
    public List<User> users { get; set; }
    public long first_event_id { get; set; }
    public long latest_event_id { get; set; }
    public object current_game_id { get; set; }
}

public class Score
{
    public double accuracy { get; set; }
    public object best_id { get; set; }
    public DateTime created_at { get; set; }
    public object id { get; set; }
    public int max_combo { get; set; }
    public string mode { get; set; }
    public int mode_int { get; set; }
    public List<string> mods { get; set; }
    public bool passed { get; set; }
    public int perfect { get; set; }
    public object pp { get; set; }
    public string rank { get; set; }
    public bool replay { get; set; }
    public int score { get; set; }
    public Statistics statistics { get; set; }
    public string type { get; set; }
    public int user_id { get; set; }
    public CurrentUserAttributes current_user_attributes { get; set; }
    public Match match { get; set; }
}

public class Statistics
{
    public int count_100 { get; set; }
    public int count_300 { get; set; }
    public int count_50 { get; set; }
    public int count_geki { get; set; }
    public int count_katu { get; set; }
    public int count_miss { get; set; }
}

public class User
{
    public string avatar_url { get; set; }
    public string country_code { get; set; }
    public string default_group { get; set; }
    public int id { get; set; }
    public bool is_active { get; set; }
    public bool is_bot { get; set; }
    public bool is_deleted { get; set; }
    public bool is_online { get; set; }
    public bool is_supporter { get; set; }
    public DateTime? last_visit { get; set; }
    public bool pm_friends_only { get; set; }
    public object profile_colour { get; set; }
    public string username { get; set; }
    public Country country { get; set; }
}

