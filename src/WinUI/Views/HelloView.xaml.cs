using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using WinUI.ViewModels;

namespace WinUI.Views
{
    /// <summary>
    /// Interaction logic for HelloView.xaml
    /// </summary>
    public partial class HelloView : Window
    {
        public HelloView()
        {
            InitializeComponent();
            this.DataContext = Ioc.Default.GetService<HelloViewModel>();
        }
    }
}
