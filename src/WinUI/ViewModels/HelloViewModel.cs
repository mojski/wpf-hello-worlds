using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.MessageBox;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System.Collections.ObjectModel;
using System.Windows;
using WinUI.Models;
using WinUI.Models.Interfaces;
using WinUI.ViewModels.FunFact;

namespace WinUI.ViewModels;

public partial class HelloViewModel : ObservableObject
{
    private readonly IDialogService dialogService;
    private readonly IFunFactService funFactService;

    private const string APPLICATION_NAME = "Hello Fun Fact Adder";

    [ObservableProperty] private bool isClosed = default;
    [ObservableProperty] private Models.FunFact selectedItem = default;
    [ObservableProperty] private ObservableCollection<Models.FunFact> items = new ();

    public HelloViewModel(IDialogService dialogService, IFunFactService funFactService)
    {
        this.dialogService = dialogService;
        this.funFactService = funFactService;

        this.UpdateFunFactCommand = new AsyncRelayCommand<Models.FunFact>(this.UpdateFunFactAsync);
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

                var funFacts = await this.funFactService.ListAsync(openFileDialogSettings.FileName, cancellationToken);

                foreach (var funFact in funFacts)
                {
                    this.Items.Add(funFact);
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
                await this.funFactService.UpdateAsync(Items, saveFileDialogSettings.FileName, cancellationToken);
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

    private async Task CreateFunFactAsync( CancellationToken cancellationToken)
    {
        var emptyModel = new Models.FunFact();
        var imagePlugin = await FileHelper.GetImagePlugAsync(cancellationToken);

        emptyModel.Image = new Image
        {
            FileName = "plugin.png",
            Value = imagePlugin,
        };

        var viewModel = new UpdateFunFactViewModel(this.dialogService) { Item = emptyModel };

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

    private async Task UpdateFunFactAsync(Models.FunFact? parameter, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        var copy = new Models.FunFact
        {
            Content = parameter.Content, 
            Title = parameter.Title, 
            Image = parameter.Image,
            Link = parameter.Link,
            RelatedMovies = parameter.RelatedMovies,
            Id = parameter.Id,
        };

        var viewModel = new UpdateFunFactViewModel(this.dialogService) { Item = copy };

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

    public IAsyncRelayCommand FileLoadCommand { get; }
    public IAsyncRelayCommand FileSaveCommand { get; }
    public IAsyncRelayCommand FileExitCommand { get; }
    public IAsyncRelayCommand<Models.FunFact> UpdateFunFactCommand { get; }
    public IAsyncRelayCommand CreateFunFactCommand { get; }
}