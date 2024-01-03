using CommunityToolkit.Mvvm.DependencyInjection;
using Syncfusion.Windows.Tools.Controls;
using WinUI.ViewModels.FunFact;

namespace WinUI.Views.FunFact
{
  public partial class UpdateFunFactView : RibbonWindow
  {
    public UpdateFunFactView()
    {
      InitializeComponent();
      this.DataContext = Ioc.Default.GetService<UpdateFunFactViewModel>();
    }
    public UpdateFunFactViewModel ViewModel => (UpdateFunFactViewModel)this.DataContext;
  }
}
