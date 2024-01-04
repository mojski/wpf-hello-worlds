using System.IO;
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
        var licensePath = "synfusion.license";
        if (File.Exists(licensePath))
        {
            var license = File.ReadAllText(licensePath);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(license);
        }
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