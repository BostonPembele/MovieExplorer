using MovieExplorer.Models;

namespace MovieExplorer.Pages;

[QueryProperty(nameof(Movie), "Movie")]
public partial class MovieDetailsPage : ContentPage
{
    private Movie _movie = new();

    public Movie Movie
    {
        get => _movie;
        set
        {
            _movie = value ?? new Movie();
            BindingContext = _movie;
        }
    }

    public MovieDetailsPage()
    {
        InitializeComponent();
        BindingContext = _movie;
    }
}
