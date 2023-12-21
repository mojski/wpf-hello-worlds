using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;

namespace WinUI.ViewModels.FunFact
{
    public partial class DetailsFunFactViewModel : ObservableObject, IModalDialogViewModel
    {
        public bool? DialogResult { get; private set; } = default;

        [ObservableProperty] private string windowTitle = "Funfact details";
        [ObservableProperty] private bool isClosed = default;
        [ObservableProperty] private Models.FunFact item = default;


        public DetailsFunFactViewModel()
        {
            this.OkCommand = new AsyncRelayCommand(this.OkAsync);
            this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);
        }

        private async Task CancelAsync()
        {
            this.IsClosed = true;
            await Task.CompletedTask;
        }

        private async Task OkAsync()
        {
            this.DialogResult = true;
            this.IsClosed = true;
            await Task.CompletedTask;
        }

        public IAsyncRelayCommand OkCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }
    }
}
