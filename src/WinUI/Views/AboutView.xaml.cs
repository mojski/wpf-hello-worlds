using Syncfusion.Windows.Tools.Controls;
using WinUI.ViewModels;

namespace WinUI.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : RibbonWindow
    {
        public AboutView()
        {
            InitializeComponent();
            this.DataContext = new AboutViewModel();
        }
    }
}
