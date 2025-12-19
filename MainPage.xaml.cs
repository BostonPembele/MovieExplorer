using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer;

public partial class MainPage : ContentPage
{
    private readonly MovieDataService _movieService = new();
    private List<Movie> _allMovies = new();
    private string _selectedGenre = "All";
    private string _selectedSort = "Title A-Z";

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

        SortPicker.ItemsSource = new List<string>
        {
            "Title A-Z",
            "Year (New-Old)",
            "Year (Old-New)",
            "IMDb (High-Low)"
        };
        SortPicker.SelectedIndex = 0;

        ApplyFilters();
    }

    private void OnSearchChanged(object sender, TextChangedEventArgs e) => ApplyFilters();

    private void OnGenreChanged(object sender, EventArgs e)
    {
        _selectedGenre = GenrePicker.SelectedItem?.ToString() ?? "All";
        ApplyFilters();
    }

    private void OnSortChanged(object sender, EventArgs e)
    {
        _selectedSort = SortPicker.SelectedItem?.ToString() ?? "Title A-Z";
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var text = (SearchEntry.Text ?? "").Trim().ToLowerInvariant();

        var query = _allMovies.Where(m =>
        {
            bool matchesTitle = string.IsNullOrWhiteSpace(text) || m.Title.ToLowerInvariant().Contains(text);
            bool matchesGenre = _selectedGenre == "All" || m.Genres.Contains(_selectedGenre);
            return matchesTitle && matchesGenre;
        });

        query = _selectedSort switch
        {
            "Year (New-Old)" => query.OrderByDescending(m => m.Year),
            "Year (Old-New)" => query.OrderBy(m => m.Year),
            "IMDb (High-Low)" => query.OrderByDescending(m => m.ImdbRating),
            _ => query.OrderBy(m => m.Title)
        };

        MoviesView.ItemsSource = query.ToList();
    }

    private async void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection?.FirstOrDefault() is not Movie movie)
            return;

        MoviesView.SelectedItem = null;

        await DisplayAlert(movie.Title, $"{movie.Year}\n{movie.GenresDisplay}", "OK");
    }
}
