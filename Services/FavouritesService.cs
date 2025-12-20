using System.Text.Json;
using MovieExplorer.Models;

namespace MovieExplorer.Services;

public class FavouritesService
{
    private const string FileName = "favourites.json";
    private readonly string _path = Path.Combine(FileSystem.AppDataDirectory, FileName);
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task<List<FavouriteEntry>> LoadAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (!File.Exists(_path))
                return new List<FavouriteEntry>();

            var json = await File.ReadAllTextAsync(_path);
            var list = JsonSerializer.Deserialize<List<FavouriteEntry>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return list ?? new List<FavouriteEntry>();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<bool> IsFavouriteAsync(string title, int year)
    {
        var list = await LoadAsync();
        return list.Any(f => f.Title == title && f.Year == year);
    }

    public async Task<bool> ToggleAsync(MovieExplorer.Models.Movie movie)
    {
        await _lock.WaitAsync();
        try
        {
            List<FavouriteEntry> list;

            if (File.Exists(_path))
            {
                var json = await File.ReadAllTextAsync(_path);
                list = JsonSerializer.Deserialize<List<FavouriteEntry>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            else
            {
                list = new List<FavouriteEntry>();
            }

            var existing = list.FirstOrDefault(f => f.Title == movie.Title && f.Year == movie.Year);

            if (existing != null)
            {
                list.Remove(existing);
                var outJsonRemove = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_path, outJsonRemove);
                return false;
            }

            list.Insert(0, new FavouriteEntry
            {
                Title = movie.Title,
                Year = movie.Year,
                Genres = movie.GenresDisplay,
                Emoji = movie.Emoji,
                TimestampUtc = DateTime.UtcNow
            });

            var outJsonAdd = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_path, outJsonAdd);
            return true;
        }
        finally
        {
            _lock.Release();
        }
    }
}
