using System.Windows.Shell;

using Uno.UI.Runtime.Skia.Wpf;

using WpfApp = System.Windows.Application;
using WpfThickness = System.Windows.Thickness;
using WpfCornerRadius = System.Windows.CornerRadius;

namespace BlankUnoApp.WPF;
public partial class App : WpfApp
{
    public App()
    {
        //var w = new System.Windows.Window
        //{
        //    WindowStyle = System.Windows.WindowStyle.None,
        //    AllowsTransparency = true,
        //    Background = System.Windows.Media.Brushes.Transparent,
        //    Content = new System.Windows.Controls.TextBlock
        //    {
        //        Text = "Hello Uno Platform!",
        //        Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red),
        //        FontSize = 24,
        //    },
        //};
        //w.Show();

        var host = new WpfHost(Dispatcher, () => new AppHead());
        PlatformWindow.Current = new WpfWindow(host);
        host.Run();
    }
}

internal class WpfWindow : IPlatformWindow
{
    private readonly WpfHost _host;

    public WpfWindow(WpfHost host)
    {
        _host = host;
    }

    public void Initialize()
    {
        var window = WpfApp.Current.MainWindow ?? throw new InvalidOperationException("Window is not set");

        window.Title = "Walterlv Blank Uno App";

        window.WindowStyle = System.Windows.WindowStyle.None;
        window.AllowsTransparency = true;
        window.Background = System.Windows.Media.Brushes.Transparent;
        //window.ResizeMode = System.Windows.ResizeMode.CanMinimize;
        //window.WindowStyle = System.Windows.WindowStyle.None;
        //window.Background = System.Windows.Media.Brushes.Transparent;
        //WindowChrome.SetWindowChrome(window, new WindowChrome
        //{
        //    CaptionHeight = 0,
        //    CornerRadius = new WpfCornerRadius(0),
        //    GlassFrameThickness = new WpfThickness(-1),
        //    NonClientFrameEdges = NonClientFrameEdges.None,
        //    ResizeBorderThickness = new WpfThickness(0),
        //    UseAeroCaptionButtons = true,
        //});
    }
}
