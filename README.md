### Application entrypoint
View: App.xaml
------------------------------------------------------------------------------------
```xml
<Application x:Class="WinUI.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="/Views/HelloView.xaml"> <!--set startup window (it was MainWindow.xaml default)-->
    <Application.Resources>
         
    </Application.Resources>
</Application>
```

And its code behind

```csharp
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IServiceCollection services = new ServiceCollection();
        // register view models
        services.AddSingleton<HelloViewModel>();

        services.AddSingleton<IDialogTypeLocator, DialogTypeLocator>();
        services.AddSingleton<IDialogService, DialogService>();

        IServiceProvider provider = services.BuildServiceProvider();

        Ioc.Default.ConfigureServices(provider);
    }
}
```
------------------------------------------------------------------------------------

Add Startup viewModel and startup View

```xml
<Window
    x:Class="WinUI.Views.HelloView" 
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:viewModels="clr-namespace:WinUI.ViewModels"
    Title="HelloView"
    Width="800"
    Height="450"
    d:DataContext="{d:DesignInstance viewModels:HelloViewModel, IsDesignTimeCreatable=True}"
    mc:Ignorable="d">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>
        <Menu Grid.Row="0">
            <MenuItem Header="Close Window">
                <MenuItem Command="{Binding Path=CloseWindowCommand, Mode=OneWay}" Header="Close" /> <!-- close window command from HelloViewModel -->
            </MenuItem>
        </Menu>
    </Grid>
</Window>
```
code begind:
```csharp
public partial class HelloView : Window
{
    public HelloView()
    {
        InitializeComponent();
        this.DataContext = Ioc.Default.GetService<HelloViewModel>(); // set data context to bing view model with view
    }
}
```

Add startup View model
```csharp
partial class HelloViewModel : ObservableObject
{
    [ObservableProperty] private bool isClosed = default;

    public HelloViewModel()
    {
        this.CloseWindowCommand = new AsyncRelayCommand(this.CloseWindowAsync);
    }

    private async Task CloseWindowAsync(CancellationToken cancellationToken)
    {
        IsClosed = true;
        await Task.CompletedTask;
    }

    public IAsyncRelayCommand CloseWindowCommand { get; }
}
```

At this point 94 line break point will hit but set IsClosed to true will have no impact to application
We need to add behavior 
- add windows behavior pacage if it has not been added yet

```shell
dotnet add package Microsoft.Xaml.Behaviors.Wpf
```

add folder Behaviors with WindowsBehavior class:

```csharp
public static class WindowBehavior
{
    private static readonly Type ownerType = typeof(WindowBehavior);

    public static readonly DependencyProperty CloseProperty =
        DependencyProperty.RegisterAttached(
            "Close",
            typeof(bool),
            ownerType,
            new UIPropertyMetadata(defaultValue: false, (sender, e) =>
            {
                if (!(e.NewValue is bool) || !(bool)e.NewValue)
                {
                    return;
                }

                Window? window = sender as Window ?? Window.GetWindow(sender);

                window?.Close();
            }));

    [AttachedPropertyBrowsableForType(typeof(Window))] // add SetCloseAction to any window class 
    public static void SetClose(DependencyObject target, bool value)
    {
        target.SetValue(CloseProperty, value);
    }
}   
```
then add in HelloView in <Window> tag attribute: xmlns:behaviors="clr-namespace:Preacon.WinUI.Behaviors" (ex afeter line 43)
it is pointer to folder (namespace) and all it behavior classes

then add <Window.Style> tag to consume behavior

```xml
<Window.Style>
    <Style>
        <Style.Triggers>
            <DataTrigger Binding="{Binding Path=IsClosed}" Value="true"> 
            <!-- change value IsClosed within HelloViewModel on true will trigger WindowBehavior's close method -->
                <Setter Property="behaviors:WindowBehavior.Close" Value="true" />
            </DataTrigger>
        </Style.Triggers>
    </Style>
</Window.Style>
```

### other parts will be on the next branch named: 1-using-MvvmDialogs-package