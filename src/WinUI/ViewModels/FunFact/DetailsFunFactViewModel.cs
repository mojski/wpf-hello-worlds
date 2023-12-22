using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System.Windows;

namespace WinUI.ViewModels.FunFact
{
    public partial class DetailsFunFactViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService dialogService;        
        private string fileBasePath = default;

        public bool? DialogResult { get; private set; } = default;

        [ObservableProperty] private string windowTitle = "FunFact details";
        [ObservableProperty] private bool isClosed = default;
        [ObservableProperty] private Models.FunFact item = default;


        public DetailsFunFactViewModel(IDialogService dialogService, string fileBasePath)
        {
            this.dialogService = dialogService;

            this.fileBasePath = fileBasePath;

            this.CloseCommand = new AsyncRelayCommand(this.CloseAsync);
            this.CopyCommand = new AsyncRelayCommand<string>(this.CopyAsync);
            this.UpdateFunFactCommand = new AsyncRelayCommand<Models.FunFact>(this.UpdateFunFactAsync);
        }

        private async Task CloseAsync(CancellationToken cancellationToken = default)
        {
            this.DialogResult = true;
            this.IsClosed = true;
            await Task.CompletedTask;
        }

        private async Task CopyAsync(string parameter, CancellationToken cancellationToken = default)
        {
            Clipboard.SetText(parameter);
            await Task.CompletedTask;
        }

        private async Task UpdateFunFactAsync(Models.FunFact? parameter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(parameter);

            var copy = new Models.FunFact
            {
                Content = parameter.Content, 
                Title = parameter.Title, 
                Image = parameter.Image,
                Link = parameter.Link,
                Id = parameter.Id,
            };

            var viewModel = new UpdateFunFactViewModel(this.dialogService, this.fileBasePath) { Item = copy };

            var result = this.dialogService.ShowDialog(this, viewModel);

            if (result is true)
            {
                parameter.Title = viewModel.Item.Title;
                parameter.Content = viewModel.Item.Content;
                parameter.Link = viewModel.Item.Link;
                parameter.Image = viewModel.Item.Image;
                parameter.RelatedMovies = viewModel.Item.RelatedMovies;
            }

            await Task.CompletedTask;
        }

        public IAsyncRelayCommand CloseCommand { get; }
        public IAsyncRelayCommand<string> CopyCommand { get; }
        public IAsyncRelayCommand<Models.FunFact> UpdateFunFactCommand { get; }
    }
}
