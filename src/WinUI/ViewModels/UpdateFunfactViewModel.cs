using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using WinUI.Models;

namespace WinUI.ViewModels;

public partial class UpdateFunfactViewModel : ObservableObject, IModalDialogViewModel
{
    [ObservableProperty] private string windowTitle = "UpdateWindow";
    [ObservableProperty] private bool isClosed = default;
    [ObservableProperty] private Funfact item = default;

    public bool? DialogResult { get; private set; } = default;

    public UpdateFunfactViewModel()
    {
        this.OkCommand = new AsyncRelayCommand(this.OkAsync);
        this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);

        // create test movie for update
        var movie = new Funfact
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

    private async Task UpdateAsync(Funfact? parameters)
    {
        this.DialogResult = true;
        this.IsClosed = true;
        await Task.CompletedTask;
    }

    public IAsyncRelayCommand OkCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }
    
}
