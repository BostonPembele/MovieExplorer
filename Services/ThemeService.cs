namespace MovieExplorer.Services;

public static class ThemeService
{
    private const string ThemeKey = "app_theme"; // "System" | "Light" | "Dark"

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
            _ => AppTheme.Unspecified // System
        };
    }

    public static string GetSavedTheme() =>
        Preferences.Get(ThemeKey, "System");
}