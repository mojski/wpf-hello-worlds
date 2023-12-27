using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.Models;

public partial class FunFact : ObservableObject
{
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string image = string.Empty;
    [ObservableProperty] private string link = string.Empty;
    [ObservableProperty] private string content = string.Empty;
    [ObservableProperty] private ObservableCollection<RelatedMovie> relatedMovies = new();
    [ObservableProperty] private int? id = default;
}