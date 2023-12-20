using System.Windows;

namespace WinUI.Behaviors;

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