using System.Net;
using System.Text.Json.Serialization;

namespace OsuMultiAnalyser;

public class Lobby
{
    public Match? match { get; set; }
    public List<Event>? events { get; set; }
    public List<User>? users { get; set; }
    public long first_event_id { get; set; }
    public long latest_event_id { get; set; }
    public object? current_game_id { get; set; }

    public static Lobby Parse(int id)
    {
        string multi_link = @"https://osu.ppy.sh/community/matches/";
        string base_uri = multi_link + id.ToString();

        using (var client = new HttpClient())
        {
            var request = new HttpRequestMessage(HttpMethod.Get, base_uri);
            request.Headers.Add("Accept", @"application/json, text/javascript, */*; q=0.01");
            var response = client.Send(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("not ok");
            }

            string json = response.Content.ReadAsStringAsync().Result;
            var lobby = System.Text.Json.JsonSerializer.Deserialize<Lobby>(json) ?? throw new Exception("failed to deserialise");
            _ = lobby.events ?? throw new Exception("no events");
            _ = lobby.users ?? throw new Exception("no users");


            var lobbyFirstEventIdSaved = lobby.events[0].id ?? throw new NullReferenceException();
            var lobbyFirstEventId = lobby.events[0].id ?? throw new NullReferenceException();
            while (lobby.events[0].id != lobby.first_event_id)
            {
                var newRequest = new HttpRequestMessage(HttpMethod.Get, base_uri + GenerateAdditionalQueryString(lobbyFirstEventId).ToString());
                newRequest.Headers.Add("Accept", @"application/json, text/javascript, */*; q=0.01");

                var newReponse = client.Send(newRequest);
                if (newReponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("not ok");
                }

                var newJson = newReponse.Content.ReadAsStringAsync().Result;
                var newLobby = System.Text.Json.JsonSerializer.Deserialize<Lobby>(newJson) ?? throw new Exception("failed to deserialise");
                _ = newLobby.events ?? throw new Exception("no events");
                _ = newLobby.users ?? throw new Exception("no users");
                lobby.events.InsertRange(0, newLobby.events);

                foreach (var user in newLobby.users)
                {
                    if (lobby.users.Find(users => users.id == user.id) == null)
                    {
                        lobby.users.Add(user);
                    }
                }

                lobbyFirstEventId = newLobby.events[0].id ?? throw new Exception("null or something");
            }

            return lobby;
        }
    }

    public static string GenerateAdditionalQueryString(long event_id)
    {
        return @"?before=" + event_id.ToString() + @"&limit=100";
    }
}

public class Beatmap
{
    public long beatmapset_id { get; set; }
    public float difficulty_rating { get; set; }
    public long id { get; set; }
    public string? mode { get; set; }
    public string? status { get; set; }
    public long total_length { get; set; }
    public long user_id { get; set; }
    public string? version { get; set; }
    public Beatmapset? beatmapset { get; set; }
}

public class Beatmapset
{
    public string? artist { get; set; }
    public string? artist_unicode { get; set; }
    public Covers? covers { get; set; }
    public string? creator { get; set; }
    public int favourite_count { get; set; }
    public object? hype { get; set; }
    public long id { get; set; }
    public bool nsfw { get; set; }
    public long offset { get; set; }
    public long play_count { get; set; }
    public string? preview_url { get; set; }
    public string? source { get; set; }
    public bool spotlight { get; set; }
    public string? status { get; set; }
    public string? title { get; set; }
    public string? title_unicode { get; set; }
    public long? track_id { get; set; }
    public long user_id { get; set; }
    public bool video { get; set; }
}

public class Country
{
    public string? code { get; set; }
    public string? name { get; set; }
}

public class Covers
{
    public string? cover { get; set; }

    [JsonPropertyName("cover@2x")]
    public string? cover2x { get; set; }
    public string? card { get; set; }

    [JsonPropertyName("card@2x")]
    public string? card2x { get; set; }
    public string? list { get; set; }

    [JsonPropertyName("list@2x")]
    public string? list2x { get; set; }
    public string? slimcover { get; set; }

    [JsonPropertyName("slimcover@2x")]
    public string? slimcover2x { get; set; }
}

public class CurrentUserAttributes
{
    public object? pin { get; set; }
}

public class Detail
{
    public string? type { get; set; }
    public string? text { get; set; }
}

public class Event
{
    public long? id { get; set; }
    public Detail? detail { get; set; }
    public DateTime timestamp { get; set; }
    public long? user_id { get; set; }
    public Game? game { get; set; }
}

public class Game
{
    public long beatmap_id { get; set; }
    public long id { get; set; }
    public DateTime? start_time { get; set; }
    public DateTime? end_time { get; set; }
    public string? mode { get; set; }
    public int mode_int { get; set; }
    public string? scoring_type { get; set; }
    public string? team_type { get; set; }
    public List<string>? mods { get; set; }
    public Beatmap? beatmap { get; set; }
    public List<Score>? scores { get; set; }
}

public class Match
{
    public long id { get; set; }
    public DateTime start_time { get; set; }
    public DateTime end_time { get; set; }
    public string? name { get; set; }
    public int slot { get; set; }
    public string? team { get; set; }
    public bool pass { get; set; }
}

public class Score
{
    public double accuracy { get; set; }
    public object? best_id { get; set; }
    public DateTime created_at { get; set; }
    public object? id { get; set; }
    public long max_combo { get; set; }
    public string? mode { get; set; }
    public int mode_int { get; set; }
    public List<string>? mods { get; set; }
    public bool passed { get; set; }
    public int perfect { get; set; }
    public object? pp { get; set; }
    public string? rank { get; set; }
    public bool replay { get; set; }
    public int score { get; set; }
    public Statistics? statistics { get; set; }
    public string? type { get; set; }
    public long user_id { get; set; }
    public CurrentUserAttributes? current_user_attributes { get; set; }
    public Match? match { get; set; }
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
    public string? avatar_url { get; set; }
    public string? country_code { get; set; }
    public string? default_group { get; set; }
    public long id { get; set; }
    public bool is_active { get; set; }
    public bool is_bot { get; set; }
    public bool is_deleted { get; set; }
    public bool is_online { get; set; }
    public bool is_supporter { get; set; }
    public DateTime? last_visit { get; set; }
    public bool pm_friends_only { get; set; }
    public object? profile_colour { get; set; }
    public string? username { get; set; }
    public Country? country { get; set; }
}

