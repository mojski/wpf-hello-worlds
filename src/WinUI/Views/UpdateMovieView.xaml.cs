using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using WinUI.ViewModels;

namespace WinUI.Views;

public partial class UpdateMovieView : Window
{
    public UpdateMovieView()
    {
        InitializeComponent();
        this.DataContext = Ioc.Default.GetService<UpdateFunfactViewModel>();
    }
}