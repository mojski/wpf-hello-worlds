using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;

namespace WinUI.ViewModels.FunFact;

public partial class UpdateFunFactViewModel : ObservableObject, IModalDialogViewModel
{
    private readonly IDialogService dialogService;

    private string fileBasePath = default;

    public UpdateFunFactViewModel(IDialogService dialogService, string dataBasePath)
    {
        this.dialogService = dialogService;
        this.fileBasePath = dataBasePath;

        this.ImageUploadCommand = new AsyncRelayCommand(this.UploadImageAsync);

        this.OkCommand = new AsyncRelayCommand(this.OkAsync);
        this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);
    }

    [ObservableProperty] private string windowTitle = "UpdateWindow";
    [ObservableProperty] private bool isClosed = default;
    [ObservableProperty] private Models.FunFact item = default;

    public bool? DialogResult { get; private set; } = default;

    private async Task UploadImageAsync(CancellationToken cancellationToken = default)
    {
        var openImageDialogSettings = new OpenFileDialogSettings
        {
            CheckFileExists = true, 
            Filter = "PNG Files (*.png)|*.png",
        };

        var result = this.dialogService.ShowOpenFileDialog(this, openImageDialogSettings);

        if (result is true)
        {
            var fileName = FileHelper.GetFullFileName(openImageDialogSettings.FileName);

            var bitmap = await FileHelper.GetBitmapImageAsync(openImageDialogSettings.FileName, cancellationToken);

            this.Item.Image.FileName = fileName;
            this.Item.Image.Value = bitmap;
        }

        await Task.CompletedTask;
    }

    private async Task CancelAsync(CancellationToken cancellationToken = default)
    {
        this.DialogResult = false;
        this.IsClosed = true;
        await Task.CompletedTask;
    }

    private async Task OkAsync(CancellationToken cancellationToken = default)
    {
        this.DialogResult = true;
        this.IsClosed = true;
        await Task.CompletedTask;
    }

    public IAsyncRelayCommand ImageUploadCommand { get; }
    public IAsyncRelayCommand OkCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }
}
