using Microsoft.Maui.Controls;

namespace MovieExplorer.Services;

public static class ThemeService
{
    private const string ThemeKey = "app_theme";

    public static void ApplySavedTheme()
    {
        var saved = Preferences.Get(ThemeKey, "System");
        ApplyTheme(saved);
    }

    public static void ApplyTheme(string theme)
    {
        Preferences.Set(ThemeKey, theme);

        if (Application.Current == null)
            return;

        Application.Current.UserAppTheme = theme switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }

    public static string GetSavedTheme()
    {
        return Preferences.Get(ThemeKey, "System");
    }
}
