namespace MovieExplorer.Models;

public class HistoryEntry
{
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public string Genres { get; set; } = "";
    public string Emoji { get; set; } = "";
    public string Action { get; set; } = "";
    public DateTime TimestampUtc { get; set; }
}
