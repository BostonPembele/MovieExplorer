namespace MovieExplorer.Models;

public class Movie
{
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public string Genres { get; set; } = "";
    public string Director { get; set; } = "";
    public double ImdbRating { get; set; }
    public string Emoji { get; set; } = "🎬";

    public List<string> GenresList =>
        Genres.Split(',', StringSplitOptions.RemoveEmptyEntries)
              .Select(g => g.Trim())
              .ToList();

    public string GenresDisplay => Genres;
}
