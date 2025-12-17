using MovieExplorer.Services;

namespace MovieExplorer.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        ThemePicker.ItemsSource = new List<string> { "System", "Light", "Dark" };

        var saved = ThemeService.GetSavedTheme();
        ThemePicker.SelectedItem = saved;
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        var theme = ThemePicker.SelectedItem?.ToString() ?? "System";
        ThemeService.ApplyTheme(theme);
    }
}
