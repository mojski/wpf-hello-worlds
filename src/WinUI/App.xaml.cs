using WinUI.Models.Interfaces;
using WinUI.Models.Services;

namespace WinUI;

using WinUI.ViewModels.FunFact;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs.DialogTypeLocators;
using MvvmDialogs;
using System.Windows;
using WinUI.ViewModels;


public partial class App : Application
{
    public App()
    {
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWX5ec3RSQmlYV0B1X0o=");
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IDialogTypeLocator, DialogTypeLocator>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IFunFactService, FunFactService>();

        services.AddSingleton<HelloViewModel>();
        services.AddSingleton<UpdateFunFactViewModel>();
        services.AddSingleton<string>(string.Empty);
        IServiceProvider provider = services.BuildServiceProvider();

        Ioc.Default.ConfigureServices(provider);
    }
}