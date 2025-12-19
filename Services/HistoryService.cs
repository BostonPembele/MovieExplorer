using System.Text.Json;
using MovieExplorer.Models;

namespace MovieExplorer.Services;

public class HistoryService
{
    private const string FileName = "history.json";
    private readonly string _path = Path.Combine(FileSystem.AppDataDirectory, FileName);
    private readonly SemaphoreSlim _lock = new(1, 1);

    public async Task<List<HistoryEntry>> LoadAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (!File.Exists(_path))
                return new List<HistoryEntry>();

            var json = await File.ReadAllTextAsync(_path);
            var list = JsonSerializer.Deserialize<List<HistoryEntry>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return list ?? new List<HistoryEntry>();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task AddAsync(Movie movie, string action)
    {
        await _lock.WaitAsync();
        try
        {
            List<HistoryEntry> list;

            if (File.Exists(_path))
            {
                var json = await File.ReadAllTextAsync(_path);
                list = JsonSerializer.Deserialize<List<HistoryEntry>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            else
            {
                list = new List<HistoryEntry>();
            }

            list.Insert(0, new HistoryEntry
            {
                Title = movie.Title,
                Year = movie.Year,
                Genres = movie.GenresDisplay,
                Emoji = movie.Emoji,
                Action = action,
                TimestampUtc = DateTime.UtcNow
            });

            var outJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_path, outJson);
        }
        finally
        {
            _lock.Release();
        }
    }
}
