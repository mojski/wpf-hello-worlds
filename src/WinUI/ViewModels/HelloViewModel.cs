using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.MessageBox;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using WinUI.ViewModels.FunFact;

namespace WinUI.ViewModels
{
    public partial class HelloViewModel : ObservableObject
    {
        private readonly IDialogService dialogService;

        private const string APPLICATION_NAME = "Hello Fun Fact Added";

        [ObservableProperty] private string fileBasePath = default;

        [ObservableProperty] private bool isClosed = default;
        [ObservableProperty] private Models.FunFact? selectedItem = default;
        [ObservableProperty] private ObservableCollection<Models.FunFact> items = new ();

        public HelloViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            this.CloseWindowCommand = new AsyncRelayCommand(this.CloseWindowAsync);
            this.ShowDetailsCommand = new AsyncRelayCommand<Models.FunFact>(this.ShowDetailsAsync);
            this.UpdateMovieCommand = new AsyncRelayCommand<Models.FunFact>(this.UpdateMovieAsync);

            this.FileLoadCommand = new AsyncRelayCommand(this.FileLoadAsync);
            this.FileSaveCommand = new AsyncRelayCommand(this.FileSaveAsync);
            this.FileExitCommand = new AsyncRelayCommand(this.FileExitAsync);
        }

        private async Task CloseWindowAsync(CancellationToken cancellationToken)
        {
            IsClosed = true;
            await Task.CompletedTask;
        }

        private async Task UpdateMovieAsync(Models.FunFact? parameter, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(parameter);

            var viewModel = new UpdateFunFactViewModel() { Item = this.selectedItem };
            viewModel.Item.Title = parameter.Title;

            var result = this.dialogService.ShowDialog(this, viewModel);

            if (result is true)
            {
                var x = viewModel.Item;
            }

            this.selectedItem.Title = viewModel.Item.Title;
            this.selectedItem.Content = viewModel.Item.Content;
            this.selectedItem.Link = viewModel.Item.Link;
            this.selectedItem.Image = viewModel.Item.Image;
            this.selectedItem.RelatedMovies = viewModel.Item.RelatedMovies;

            await Task.CompletedTask;
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
                    var fileContent = await File.ReadAllTextAsync(openFileDialogSettings.FileName, cancellationToken);
                    items.Clear();
                    var funFacts =  JsonConvert.DeserializeObject<ObservableCollection<Models.FunFact>>(fileContent);

                    foreach (var funFact in funFacts)
                    {
                        items.Add(funFact);
                    }

                    var fileDirectory = new FileInfo(openFileDialogSettings.FileName).Directory.FullName;
                    this.fileBasePath = fileDirectory;
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
                    var json = JsonConvert.SerializeObject(items);

                    if (File.Exists(saveFileDialogSettings.FileName))
                    {
                        File.Delete(saveFileDialogSettings.FileName);
                    }

                    await File.WriteAllTextAsync(saveFileDialogSettings.FileName, json, cancellationToken);
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

            var viewModel = new DetailsFunFactViewModel() { Item = this.selectedItem };
            var imageFileName = Path.GetFileName(parameter.Image);
            viewModel.Item.Image = Path.Combine(fileBasePath, "images", imageFileName);

            var result = this.dialogService.ShowDialog(this, viewModel);

            await Task.CompletedTask;
        }

        public IAsyncRelayCommand CloseWindowCommand { get; }
        public IAsyncRelayCommand<Models.FunFact> UpdateMovieCommand { get; }
        public IAsyncRelayCommand FileLoadCommand { get; }
        public IAsyncRelayCommand FileSaveCommand { get; }
        public IAsyncRelayCommand FileExitCommand { get; }
        public IAsyncRelayCommand<Models.FunFact> ShowDetailsCommand { get; }
    }
}
