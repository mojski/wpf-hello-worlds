using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.Models;

public partial class Funfact : ObservableObject
{
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string image = string.Empty;
    [ObservableProperty] private string link = string.Empty;
    [ObservableProperty] private string content = string.Empty;
    [ObservableProperty] private string relatedMovies = string.Empty;

    public Guid Id { get; set; } = Guid.Empty;
}