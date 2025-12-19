using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer.Pages;

public partial class HistoryPage : ContentPage
{
    private readonly HistoryService _historyService = new();

    public HistoryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var items = await _historyService.LoadAsync();

        var viewModels = items.Select(h => new HistoryEntryVm(h)).ToList();
        HistoryView.ItemsSource = viewModels;
    }

    private class HistoryEntryVm
    {
        private readonly HistoryEntry _h;

        public HistoryEntryVm(HistoryEntry h) => _h = h;

        public string Title => _h.Title;
        public int Year => _h.Year;
        public string Genres => _h.Genres;
        public string Emoji => _h.Emoji;
        public string Action => _h.Action;

        public string TimestampLocal =>
            DateTime.SpecifyKind(_h.TimestampUtc, DateTimeKind.Utc)
                .ToLocalTime()
                .ToString("dd MMM yyyy, HH:mm");
    }
}
