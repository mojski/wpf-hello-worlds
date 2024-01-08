using System.ComponentModel;
using MvvmDialogs;

namespace WinUI.ViewModels;

internal sealed class AboutViewModel : IModalDialogViewModel
{
    public event PropertyChangedEventHandler PropertyChanged;
    public bool? DialogResult { get; } = default;
}