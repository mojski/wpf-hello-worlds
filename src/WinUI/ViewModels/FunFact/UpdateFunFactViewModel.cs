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

        this.OkCommand = new AsyncRelayCommand(this.OkAsync);
        this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);

        this.ImageUploadCommand = new AsyncRelayCommand(this.UploadImageAsync);
    }

    [ObservableProperty] private string windowTitle = "UpdateWindow";
    [ObservableProperty] private bool isClosed = default;
    [ObservableProperty] private Models.FunFact item = default;

    public bool? DialogResult { get; private set; } = default;

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

            var funFactFilePath = Path.Combine("images", fileName);
            var movePath = Path.Combine(this.fileBasePath, funFactFilePath);
            item.Image = funFactFilePath;

            File.Move(openImageDialogSettings.FileName, movePath);
        }

        this.IsClosed = true;
        await Task.CompletedTask;
    }

    public IAsyncRelayCommand OkCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }
    public IAsyncRelayCommand ImageUploadCommand { get; }
}
