using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace WinUI.UserControl;

/// <summary>
///     Interaction logic for IconEmbedder.xaml
/// </summary>
public partial class IconEmbedder : System.Windows.Controls.UserControl
{
    private const string RESOURCE_PATH = @".\Resources\Assets\Icons";
    protected override void OnInitialized(EventArgs e) {
        InitializeComponent();
        base.OnInitialized(e);

        var path = Path.Combine(RESOURCE_PATH, $"{this.Icon}.xaml");
        
        StreamReader sr = new StreamReader(path);
        UIElement rootElement = (UIElement) XamlReader.Load(sr.BaseStream);
        this.AddChild(rootElement);
    }
    
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(string),
        typeof(IconEmbedder),
        new UIPropertyMetadata("default"));

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}