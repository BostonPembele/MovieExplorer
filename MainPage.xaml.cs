using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer;

public partial class MainPage : ContentPage
{
    private readonly MovieDataService _movieService = new();
    private List<Movie> _allMovies = new();
    private string _selectedGenre = "All";

    public MainPage()
    {
        InitializeComponent();
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        _allMovies = await _movieService.GetMoviesAsync();

        var genres = _allMovies
            .SelectMany(m => m.Genres)
            .Distinct()
            .OrderBy(g => g)
            .ToList();

        genres.Insert(0, "All");

        GenrePicker.ItemsSource = genres;
        GenrePicker.SelectedIndex = 0;

        ApplyFilters();
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs e) => ApplyFilters();

    private void OnGenreChanged(object sender, EventArgs e)
    {
        _selectedGenre = GenrePicker.SelectedItem?.ToString() ?? "All";
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var text = (SearchEntry.Text ?? "").Trim().ToLowerInvariant();

        var filtered = _allMovies.Where(m =>
        {
            bool matchesTitle = string.IsNullOrWhiteSpace(text) || m.Title.ToLowerInvariant().Contains(text);
            bool matchesGenre = _selectedGenre == "All" || m.Genres.Contains(_selectedGenre);
            return matchesTitle && matchesGenre;
        }).ToList();

        MoviesView.ItemsSource = filtered;
    }

    private async void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection?.FirstOrDefault() is not Movie movie)
            return;

        MoviesView.SelectedItem = null;

        await DisplayAlert(movie.Title, $"{movie.Year}\n{movie.GenresDisplay}", "OK");
    }
}
