using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;

namespace WinUI.ViewModels.FunFact;

public partial class UpdateFunFactViewModel : ObservableObject, IModalDialogViewModel
{
    [ObservableProperty] private string windowTitle = "UpdateWindow";
    [ObservableProperty] private bool isClosed = default;
    [ObservableProperty] private Models.FunFact item = default;

    public bool? DialogResult { get; private set; } = default;

    public UpdateFunFactViewModel()
    {
        this.OkCommand = new AsyncRelayCommand(this.OkAsync);
        this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);

        // create test movie for update
        var movie = new Models.FunFact
        {
            Title = "Test Title",
        };
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

    private async Task UpdateAsync(Models.FunFact? parameters)
    {
        this.DialogResult = true;
        this.IsClosed = true;
        await Task.CompletedTask;
    }

    public IAsyncRelayCommand OkCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }
    
}
