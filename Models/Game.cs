namespace OsuMultiAnalyser;

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

    public float BlueAverageScore { get; set; } = 0.0f;
    public float RedAverageScore { get; set; } = 0.0f;
}
