namespace MovieExplorer.Models;

public class Movie
{
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public List<string> Genres { get; set; } = new();
    public string Director { get; set; } = "";
    public double ImdbRating { get; set; }
    public string Emoji { get; set; } = "🎬";

    public string GenresDisplay => string.Join(", ", Genres);
}
