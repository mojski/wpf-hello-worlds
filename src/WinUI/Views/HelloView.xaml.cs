namespace WinUI.Views;

using CommunityToolkit.Mvvm.DependencyInjection;
using Syncfusion.Windows.Tools.Controls;
using WinUI.ViewModels;

public partial class HelloView : RibbonWindow
{
    public HelloViewModel ViewModel => (HelloViewModel)this.DataContext;
    public HelloView()
    {
        InitializeComponent();
        this.DataContext = Ioc.Default.GetService<HelloViewModel>();
    }
}