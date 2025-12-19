using MovieExplorer.Pages;

namespace MovieExplorer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(MovieDetailsPage), typeof(MovieDetailsPage));
    }
}
