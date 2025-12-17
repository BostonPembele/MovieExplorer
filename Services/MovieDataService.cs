using System.Text.Json;
using MovieExplorer.Models;

namespace MovieExplorer.Services;

public class MovieDataService
{
    private const string MoviesUrl =
        "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";

    private const string CacheFileName = "moviesemoji.json";

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var cachePath = Path.Combine(FileSystem.AppDataDirectory, CacheFileName);

        if (!File.Exists(cachePath))
        {
            using var client = new HttpClient();
            var json = await client.GetStringAsync(MoviesUrl);
            File.WriteAllText(cachePath, json);
        }

        var cachedJson = File.ReadAllText(cachePath);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var movies = JsonSerializer.Deserialize<List<Movie>>(cachedJson, options);
        return movies ?? new List<Movie>();
    }
}
