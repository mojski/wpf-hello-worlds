namespace WinUI;

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs.DialogTypeLocators;
using MvvmDialogs;
using System.Windows;
using WinUI.ViewModels;


public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IDialogTypeLocator, DialogTypeLocator>();
        services.AddSingleton<IDialogService, DialogService>();

        services.AddSingleton<HelloViewModel>();
        IServiceProvider provider = services.BuildServiceProvider();

        Ioc.Default.ConfigureServices(provider);
    }
}