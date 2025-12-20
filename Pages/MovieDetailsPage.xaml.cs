using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer.Pages;

[QueryProperty(nameof(Movie), "Movie")]
public partial class MovieDetailsPage : ContentPage
{
    private readonly FavouritesService _favourites = new();
    private readonly HistoryService _history = new();
    private Movie _movie = new();

    public Movie Movie
    {
        get => _movie;
        set
        {
            _movie = value ?? new Movie();
            BindingContext = _movie;
            _ = RefreshFavAsync();
        }
    }

    public MovieDetailsPage()
    {
        InitializeComponent();
        BindingContext = _movie;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshFavAsync();
    }

    private async Task RefreshFavAsync()
    {
        if (FavButton == null)
            return;

        var isFav = await _favourites.IsFavouriteAsync(_movie.Title, _movie.Year);
        FavButton.Text = isFav ? "★" : "☆";
    }

    private async void OnFavClicked(object sender, EventArgs e)
    {
        var isFavNow = await _favourites.ToggleAsync(_movie);
        FavButton.Text = isFavNow ? "★" : "☆";

        await _history.AddAsync(_movie, isFavNow ? "Favourited" : "Unfavourited");
    }
}
