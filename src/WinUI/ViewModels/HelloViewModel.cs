using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.MessageBox;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using WinUI.Models.Entities;
using WinUI.Models.Interfaces;
using WinUI.ViewModels.FunFact;

namespace WinUI.ViewModels
{
    public partial class HelloViewModel : ObservableObject
    {
        private readonly IDialogService dialogService;
        private readonly IFunFactService funFactService;

        private const string APPLICATION_NAME = "Hello Fun Fact Adder";

        [ObservableProperty] private string fileBasePath = default;
        [ObservableProperty] private bool isClosed = default;
        [ObservableProperty] private Models.FunFact selectedItem = default;
        [ObservableProperty] private ObservableCollection<Models.FunFact> items = new ();

        public HelloViewModel(IDialogService dialogService, IFunFactService funFactService)
        {
            this.dialogService = dialogService;
            this.funFactService = funFactService;

            this.ShowDetailsCommand = new AsyncRelayCommand<Models.FunFact>(this.ShowDetailsAsync);
            this.CreateFunFactCommand = new AsyncRelayCommand(this.CreateFunFactAsync);

            this.FileLoadCommand = new AsyncRelayCommand(this.FileLoadAsync);
            this.FileSaveCommand = new AsyncRelayCommand(this.FileSaveAsync);
            this.FileExitCommand = new AsyncRelayCommand(this.FileExitAsync);
        }

        private async Task FileLoadAsync(CancellationToken cancellationToken)
        {
            var openFileDialogSettings = new OpenFileDialogSettings
            {
                CheckFileExists = true, 
                Filter = "CSV Files (*.json)|*.json|All Files|*.*",
            };

            var result = this.dialogService.ShowOpenFileDialog(this, openFileDialogSettings);

            if (result is true)
            {
                try
                {
                    this.Items.Clear();

                    var entities = await this.funFactService.ListAsync(openFileDialogSettings.FileName, cancellationToken);

                    foreach (var entity in entities)
                    {
                        var model = entity.ToFunFact();
                        this.Items.Add(model);
                    }

                    var fileDirectory = new FileInfo(openFileDialogSettings.FileName).Directory.FullName;
                    this.FileBasePath = fileDirectory;
                }
                catch (Exception exception)
                {
                    var messageBoxSettings = new MessageBoxSettings
                    {
                        MessageBoxText = exception.Message,
                        Caption = APPLICATION_NAME,
                        Icon = MessageBoxImage.Error,
                        Button = MessageBoxButton.OK,
                    };

                    _ = this.dialogService.ShowMessageBox(this, messageBoxSettings);
                }
            }
        }

        private async Task FileSaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                var saveFileDialogSettings = new SaveFileDialogSettings()
                {
                    CheckPathExists = true,
                    Filter = "CSV Files (*.json)|*.json|All Files|*.*",
                    DefaultExt = ".json"
                };

                var result = this.dialogService.ShowSaveFileDialog(this, saveFileDialogSettings);

                if (result is true)
                {
                    var entities = this.Items.Select(FunFactEntity.FromFunFact);
                    await this.funFactService.UpdateAsync(entities, saveFileDialogSettings.FileName, cancellationToken);
                }
            }
            catch (Exception exception)
            {
                var messageBoxSettings = new MessageBoxSettings
                {
                    MessageBoxText = exception.Message,
                    Caption = APPLICATION_NAME,
                    Icon = MessageBoxImage.Error,
                    Button = MessageBoxButton.OK,
                };

                _ = this.dialogService.ShowMessageBox(this, messageBoxSettings);
            }
        }

        private async Task FileExitAsync(CancellationToken cancellationToken)
        {
            this.IsClosed = true;
            await Task.CompletedTask;
        }

        private async Task ShowDetailsAsync(Models.FunFact? parameter, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(parameter);

            var viewModel = new DetailsFunFactViewModel(this.dialogService, this.fileBasePath) { Item = parameter };
            var imageFileName = Path.GetFileName(parameter.Image);

            if (string.IsNullOrWhiteSpace(fileBasePath) is false)
            {
                viewModel.Item.Image = Path.Combine(fileBasePath, "images", imageFileName);
                _ = this.dialogService.ShowDialog(this, viewModel);

                await Task.CompletedTask;
            }
        }

        private async Task CreateFunFactAsync( CancellationToken cancellationToken)
        {
            if (this.FileBasePath is null || this.FileBasePath.Length == 0)
            {
                var messageBoxSettings = new MessageBoxSettings
                {
                    MessageBoxText = "You can not add fun facts to empty list.",
                    Caption = APPLICATION_NAME,
                    Icon = MessageBoxImage.Error,
                    Button = MessageBoxButton.OK,
                };

                _ = this.dialogService.ShowMessageBox(this, messageBoxSettings);

                return;
            }

            var emptyModel = new Models.FunFact();
            var viewModel = new UpdateFunFactViewModel(this.dialogService, fileBasePath) { Item = emptyModel };

            var result = this.dialogService.ShowDialog(this, viewModel);

            if (result is true)
            {
                var nextId = Items.Any() ? Items.Max(x=>x.Id) + 1 : 1;
                var newModel = new Models.FunFact
                {
                    Id = nextId,
                    Title = viewModel.Item.Title,
                    Content = viewModel.Item.Content,
                    Link = viewModel.Item.Link,
                    Image = viewModel.Item.Image,
                    RelatedMovies = viewModel.Item.RelatedMovies,
                };

                Items.Add(newModel);
            }

            await Task.CompletedTask;
        }

        public IAsyncRelayCommand FileLoadCommand { get; }
        public IAsyncRelayCommand FileSaveCommand { get; }
        public IAsyncRelayCommand FileExitCommand { get; }
        public IAsyncRelayCommand<Models.FunFact> ShowDetailsCommand { get; }
        public IAsyncRelayCommand CreateFunFactCommand { get; }
    }
}
