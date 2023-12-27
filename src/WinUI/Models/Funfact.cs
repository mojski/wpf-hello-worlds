using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.Models;

public partial class FunFact : ObservableObject
{
    private const string FUN_FACT_IMAGE_PLUGIN = "/Resources/noimg.png";

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string image = FUN_FACT_IMAGE_PLUGIN;
    [ObservableProperty] private string link = string.Empty;
    [ObservableProperty] private string content = string.Empty;
    [ObservableProperty] private ObservableCollection<RelatedMovie> relatedMovies = new();
    [ObservableProperty] private int? id = default;
}