using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using WinUI.ViewModels.FunFact;

namespace WinUI.Views.FunFact
{
  public partial class UpdateFunFactView : Window
  {
    public UpdateFunFactView()
    {
      InitializeComponent();
      this.DataContext = Ioc.Default.GetService<UpdateFunFactViewModel>();
    }
    public UpdateFunFactViewModel ViewModel => (UpdateFunFactViewModel)this.DataContext;
  }
}
