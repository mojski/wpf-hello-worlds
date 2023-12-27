using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.Models
{
    public partial class RelatedMovie : ObservableObject
    { 
        [ObservableProperty] private int id = default;
    }
}
