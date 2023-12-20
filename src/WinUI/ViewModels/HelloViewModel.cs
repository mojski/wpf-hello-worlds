using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WinUI.ViewModels
{
    partial class HelloViewModel : ObservableObject
    {
        [ObservableProperty] private bool isClosed = default;

        public HelloViewModel()
        {
            this.CloseWindowCommand = new AsyncRelayCommand(this.CloseWindowAsync);
        }

        private async Task CloseWindowAsync(CancellationToken cancellationToken)
        {
            IsClosed = true;
            await Task.CompletedTask;
        }

        public IAsyncRelayCommand CloseWindowCommand { get; }
    }
}
