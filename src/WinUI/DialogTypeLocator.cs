﻿using MvvmDialogs.DialogTypeLocators;
using System.ComponentModel;
using WinUI.ViewModels.FunFact;
using WinUI.Views.FunFact;

namespace WinUI;

internal sealed class DialogTypeLocator : IDialogTypeLocator
{
  public Type Locate(INotifyPropertyChanged viewModel) => viewModel switch
  {
    null => throw new ArgumentNullException(nameof(viewModel)),
    //register all your dialogs here (must implement IModalDialogViewModel interface
    //
    DetailsFunFactViewModel => typeof(DetailsFunFactView),
    UpdateFunFactViewModel => typeof(UpdateFunFactView),
    _ => throw new ArgumentException($"No dialog view type found for view model type {viewModel.GetType()}"),
  };
}