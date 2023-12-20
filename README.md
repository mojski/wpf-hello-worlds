### Add and use MvvmDialogs package

- add MvvmDialogs package if it has not been added yet

```shell
dotnet add package MvvmDialogs
```
Add DialogTypeLocator class

- add update view and update view model (we will use Movie example)
- register view model in App.xaml.cs:
-register DialogService and DialogTypeLocator

App.xaml.cs shoult lokks like this:

```csharp
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IServiceCollection services = new ServiceCollection();
        // register view models
        services.AddSingleton<HelloViewModel>();
        services.AddSingleton<UpdateMovieViewModel>();

        services.AddSingleton<IDialogTypeLocator, DialogTypeLocator>();
        services.AddSingleton<IDialogService, DialogService>();

        IServiceProvider provider = services.BuildServiceProvider();

        Ioc.Default.ConfigureServices(provider);
    }
}
```

- set DataContext in UpdateMovieView code behind:

```csharp
public partial class UpdateMovieView : Window
{
    public UpdateMovieView()
    {
        InitializeComponent();
        this.DataContext = Ioc.Default.GetService<UpdateMovieViewModel>();
    }
}
```

- add Movie model:
```csharp
public partial class Movie : ObservableObject
{
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string author = string.Empty;
    [ObservableProperty] private string genre = string.Empty;
}
```

- Update Movie View Model must implement IModalDialogViewModel (to be able to fine by DialogTypeLocator)

```csharp
public partial class UpdateMovieViewModel : ObservableObject, IModalDialogViewModel
{
    [ObservableProperty] private bool isClosed = default;
    [ObservableProperty] private Movie item = default;

    public bool? DialogResult { get; private set; } = default;

    public UpdateMovieViewModel()
    {
        this.OkCommand = new AsyncRelayCommand(this.OkAsync);
        this.CancelCommand = new AsyncRelayCommand(this.CancelAsync);
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

    public IAsyncRelayCommand OkCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }

}
```
- add UpdateMovieCommand prop (create it in constructor) and UpdateMovieAsync method

```csharp

```

In HellowView add button with binded command

```xml
<Button
    Command="{Binding Path=UpdateMovieCommand, Mode=OneWay}"
    CommandParameter="{Binding Path=Movie, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"
    Content="Update Movie" />
```