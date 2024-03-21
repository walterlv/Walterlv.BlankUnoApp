#if HAS_UNO
using Uno.UI.Xaml;
#endif

namespace BlankUnoApp;

public class App : Application
{
    protected Window? MainWindow { get; private set; }

    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
#if NET6_0_OR_GREATER && WINDOWS && !HAS_UNO
        MainWindow = new Window();
#else
        MainWindow = Microsoft.UI.Xaml.Window.Current;
#endif

#if DEBUG
        MainWindow.EnableHotReload();
#endif
#if HAS_UNO
        MainWindow.SetBackground(new SolidColorBrush(Colors.Transparent));
#endif
        PlatformWindow.Current.Initialize();

        MainWindow.Content = new TextBlock()
            .Text("Hello Uno Platform!")
            .FontSize(48)
            .Foreground(new SolidColorBrush(Colors.Red));

        //// Do not repeat app initialization when the Window already has content,
        //// just ensure that the window is active
        //if (MainWindow.Content is not Frame rootFrame)
        //{
        //    // Create a Frame to act as the navigation context and navigate to the first page
        //    rootFrame = new Frame();

        //    // Place the frame in the current Window
        //    MainWindow.Content = rootFrame;

        //    rootFrame.NavigationFailed += OnNavigationFailed;
        //}

        //if (rootFrame.Content == null)
        //{
        //    // When the navigation stack isn't restored navigate to the first page,
        //    // configuring the new page by passing required information as a navigation
        //    // parameter
        //    rootFrame.Navigate(typeof(MainPage), args.Arguments);
        //}

        // Ensure the current window is active
        MainWindow.Activate();

        await Task.Delay(1000);

        var w = MainWindow;
        var root = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(w.Content));
        ((Panel)root).Background = new SolidColorBrush(Colors.Transparent);

#if HAS_UNO
        MainWindow.SetBackground(new SolidColorBrush(Colors.Transparent));
#endif

        await Task.Delay(1000);
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new InvalidOperationException($"Failed to load {e.SourcePageType.FullName}: {e.Exception}");
    }
}
