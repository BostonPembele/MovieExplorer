using MovieExplorer.Services;

namespace MovieExplorer.Pages;

public partial class FavouritesPage : ContentPage
{
    private readonly FavouritesService _favourites = new();

    public FavouritesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        FavView.ItemsSource = await _favourites.LoadAsync();
    }
}
