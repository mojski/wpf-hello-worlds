using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using WinUI.ViewModels.FunFact;

namespace WinUI.Views.FunFact
{
    /// <summary>
    /// Interaction logic for DetailsFunFactView.xaml
    /// </summary>
    public partial class DetailsFunFactView : Window
    {
        public DetailsFunFactView()
        {
            InitializeComponent();
            this.DataContext = Ioc.Default.GetService<DetailsFunFactViewModel>();
        }
    }
}
