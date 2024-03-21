namespace BlankUnoApp;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this
            .Background(new SolidColorBrush(Colors.Transparent));
        //.Content(new StackPanel()
        //.VerticalAlignment(VerticalAlignment.Center)
        //.HorizontalAlignment(HorizontalAlignment.Center)
        //.Children(
        //    new TextBlock()
        //        .Text("Hello Uno Platform!")
        //        .Foreground(new SolidColorBrush(Colors.Red))
        //        .FontSize(24)
        //));
    }
}
