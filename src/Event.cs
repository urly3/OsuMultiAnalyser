namespace OsuMultiAnalyser;

public class Event
{
    public long? id { get; set; }
    public Detail? detail { get; set; }
    public DateTime timestamp { get; set; }
    public long? user_id { get; set; }
    public Game? game { get; set; }
}
