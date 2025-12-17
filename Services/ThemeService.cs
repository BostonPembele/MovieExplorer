namespace MovieExplorer.Services;

public static class ThemeService
{
    private const string ThemeKey = "app_theme"; // "System" | "Light" | "Dark"

    public static void ApplySavedTheme()
    {
        var saved = Preferences.Get(ThemeKey, "System");
        ApplyTheme(saved);
    }