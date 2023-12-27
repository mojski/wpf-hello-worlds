using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media.Imaging;

namespace WinUI.Models
{
    public partial class Image : ObservableObject
    {
        [ObservableProperty] private BitmapImage value = default;
        [ObservableProperty] private string fileName = string.Empty;

    }
}
