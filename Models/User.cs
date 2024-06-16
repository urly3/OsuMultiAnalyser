namespace OsuMultiAnalyser;

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

    // TODO:
    // come up with a representation of cost / impact.
    public int Impact { get; set; } = 0;

    public int AverageScore { get; set; } = 0;
    public int HighestScore { get; set; } = 0;
    public int LowestScore { get; set; } = 0;
    public int MapsPlayed { get; set; } = 0;

    public float AverageAccuracy { get; set; } = 0;
    public float HighestAccuracy { get; set; } = 0;
    public float LowestAccuracy { get; set; } = 0;

    public void CalculateAverageScore(List<Event> gameEvents)
    {
        int averageScore = 0;
        int scoreCount = 0;

        foreach (var scores in gameEvents.
            Where(ge => ge.game?.scores?
                .Exists(s => s.user_id == this.id) ?? false)
            .Select(ge => ge.game?.scores))
        {

            averageScore += scores?.First(s => s.user_id == this.id).score ?? 0;
            ++scoreCount;
        }

        if (scoreCount == 0) return;

        AverageScore = averageScore / scoreCount;
        MapsPlayed = scoreCount;
    }

    public void GetHighestScore(List<Event> gameEvents)
    {
        this.HighestScore = gameEvents
            .Where(ge => ge.game?.scores?
                .Exists(s => s.user_id == this.id) ?? false)
            .Select(ge => ge.game?.scores)
            .Max(s => s?.Where(s => s.user_id == this.id).Max(s => s?.score))
        ?? 0;
    }

    public void GetLowestScore(List<Event> gameEvents)
    {
        this.LowestScore = gameEvents
            .Where(ge => ge.game?.scores?
                .Exists(s => s.user_id == this.id) ?? false)
            .Select(ge => ge.game?.scores)
            .Min(s => s?.Where(s => s.user_id == this.id).Min(s => s?.score))
        ?? 0;
    }

    public void CalculateAverageAccuracy(List<Event> gameEvents)
    {
        float averageAccuracy = 0;
        int scoreCount = 0;

        foreach (var scores in gameEvents
            .Where(ge => ge.game?.scores?
                .Exists(s => s.user_id == this.id) ?? false)
            .Select(ge => ge.game?.scores))
        {

            averageAccuracy += scores?.First(s => s.user_id == this.id).accuracy ?? 0;
            ++scoreCount;
        }

        if (scoreCount == 0) return;

        AverageAccuracy = (float)Math.Round((averageAccuracy / scoreCount) * 100, 2);
        MapsPlayed = scoreCount;
    }

    public void GetHighestAccuracy(List<Event> gameEvents)
    {
        this.HighestAccuracy = (float)Math.Round(gameEvents
            .Where(ge => ge.game?.scores?
                .Exists(s => s.user_id == this.id) ?? false)
            .Select(ge => ge.game?.scores)
            .Max(s => s?.Where(s => s.user_id == this.id).Max(s => s?.accuracy)) * 100
            ?? 0.0f, 2
        );
    }

    public void GetLowestAccuracy(List<Event> gameEvents)
    {
        this.LowestAccuracy = (float)Math.Round(gameEvents
            .Where(ge => ge.game?.scores?
                .Exists(s => s.user_id == this.id) ?? false)
            .Select(ge => ge.game?.scores)
            .Min(s => s?.Where(s => s.user_id == this.id).Min(s => s?.accuracy)) * 100
            ?? 0.0f, 2
        );
    }
}