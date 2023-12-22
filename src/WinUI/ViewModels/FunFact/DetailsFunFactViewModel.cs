using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;

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

            this.OkCommand = new AsyncRelayCommand(this.OkAsync);
            this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);
            this.UpdateFunFactCommand = new AsyncRelayCommand<Models.FunFact>(this.UpdateFunFactAsync);
            this.ImageUploadCommand = new AsyncRelayCommand(this.UploadImageAsync);
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

        private async Task UpdateFunFactAsync(Models.FunFact? parameter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(parameter);

            var viewModel = new UpdateFunFactViewModel(this.dialogService, this.fileBasePath) { Item = parameter };

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

        private async Task UploadImageAsync()
        {
            var openImageDialogSettings = new OpenFileDialogSettings
            {
                CheckFileExists = true, 
                Filter = "PNG Files (*.png)|*.png",
            };

            var result = this.dialogService.ShowOpenFileDialog(this, openImageDialogSettings);

            if (result is true)
            {
                var fileName = Path.GetFileName(openImageDialogSettings.FileName);
                this.Item.Image = Path.Combine("images", fileName);
            }

            this.IsClosed = true;
            await Task.CompletedTask;
        }

        public IAsyncRelayCommand OkCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }
        public IAsyncRelayCommand<Models.FunFact> UpdateFunFactCommand { get; }
        public IAsyncRelayCommand ImageUploadCommand { get; }
    }
}
