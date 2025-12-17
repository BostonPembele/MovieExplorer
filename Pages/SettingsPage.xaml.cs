using MovieExplorer.Services;

namespace MovieExplorer.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        ThemePicker.ItemsSource = new List<string> { "System", "Light", "Dark" };
        ThemePicker.SelectedItem = ThemeService.GetSavedTheme();
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        var theme = ThemePicker.SelectedItem?.ToString() ?? "System";
        ThemeService.ApplyTheme(theme);
    }
}
