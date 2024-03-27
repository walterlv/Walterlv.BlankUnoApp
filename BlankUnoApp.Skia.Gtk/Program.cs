using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Cairo;

using Gdk;

using GLib;

using Gtk;

using SkiaSharp;

using Uno.UI.Runtime.Skia.Gtk;

using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.System.Threading.Core;

using Color = Cairo.Color;
using Task = System.Threading.Tasks.Task;

namespace BlankUnoApp.Skia.Gtk;
public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            System.Threading.Thread.Sleep(100);
            if (Debugger.IsAttached)
            {
                break;
            }
        }

        ExceptionManager.UnhandledException += delegate (UnhandledExceptionArgs expArgs)
        {
            Console.WriteLine("GLIB UNHANDLED EXCEPTION" + expArgs.ExceptionObject.ToString());
            expArgs.ExitApplication = true;
        };

        var host = new GtkHost(() => new AppHead());
        PlatformWindow.Current = new GtkWindow(host);
        host.RenderSurfaceType = RenderSurfaceType.Software;
        host.Run();

        //global::Gtk.Application.Init();
        //var window = new DemoWindow();
        //window.ShowAll();
        //global::Gtk.Application.Run();
    }
}

internal class GtkWindow : IPlatformWindow
{
    private const string libX11Ext = "libXext.so.6";
    private readonly GtkHost _host;

    public GtkWindow(GtkHost host)
    {
        _host = host;
    }

    public void Initialize()
    {
        var window = _host.Window ?? throw new InvalidOperationException("Window is not set");

        window.Title = "Walterlv Blank Uno App";

        Console.WriteLine("GtkWindow.Initialize");
        //window.AppPaintable = true;
        var screen = window.Screen;
        var visual = screen.RgbaVisual;
        Console.WriteLine("IsComposited: " + screen.IsComposited);
        Console.WriteLine("Visual: " + visual);
        if (visual is not null && screen.IsComposited)
        {
            //window.Visual = visual;
        }
        ClipInteraction(window.Display.Handle, window.Handle);
    }

    static void ClipInteraction(nint display, nint window)
    {
        //XRectangle[] rects = [
        //        new XRectangle {
        //            x = 0,
        //            y = 0,
        //            width = 100,
        //            height = 100,
        //        },
        //        new XRectangle {
        //            x = 0,
        //            y = 0,
        //            width = 100,
        //            height = 100,
        //        },
        //    ];
        //XShapeCombineRectangles(display, window, XShapeKind.ShapeBounding,
        //    0, 0, rects, 1,
        //    XShapeOperation.ShapeSet, XOrdering.YXBanded);

        var rects = new XRectangle[]
        {
            new XRectangle
            {
                x = 0,
                y = 0,
                width = 1,
                height = 1,
            }
        };
        //Console.WriteLine(rect);
        XShapeCombineRectangles(display, window, XShapeKind.ShapeBounding, 0, 0, rects, rects.Length, XShapeOperation.ShapeSet, XOrdering.Unsorted);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct XRectangle
    {
        public short x;
        public short y;
        public ushort width;
        public ushort height;
    }

    public enum XShapeOperation
    {
        ShapeSet,
        ShapeUnion,
        ShapeIntersect,
        ShapeSubtract,
        ShapeInvert
    }

    public enum XShapeKind
    {
        ShapeBounding,
        ShapeClip,
        //ShapeInput // Not usable without more imports
    }

    public enum XOrdering
    {
        Unsorted,
        YSorted,
        YXSorted,
        YXBanded
    }

    [DllImport("libXext.so.6")]
    public extern static void XShapeCombineRectangles(IntPtr display, IntPtr window,
        XShapeKind dest_kind, int x_off, int y_off,
        XRectangle[] rectangles, int n_rects,
        XShapeOperation op, XOrdering ordering);
    //[DllImport(libX11Ext)]
    //public extern static void XShapeCombineRectangles(IntPtr display, IntPtr window,
    //    XShapeKind dest_kind, int x_off, int y_off,
    //    XRectangle[] rectangles, int n_rects,
    //    XShapeOperation op, XOrdering ordering);
}

internal sealed class DemoWindow : global::Gtk.Window
{
    public DemoWindow() : base(global::Gtk.WindowType.Toplevel)
    {
        Title = "Walterlv Blank Gtk App";
        SetDefaultSize(800, 600);
        Add(new Area());

        this.AppPaintable = true;
        var screen = this.Screen;
        var visual = screen.RgbaVisual;
        if (visual is not null && screen.IsComposited)
        {
            this.Visual = visual;
        }
    }

    protected override bool OnDeleteEvent(Event evnt)
    {
        global::Gtk.Application.Quit();
        return base.OnDeleteEvent(evnt);
    }
}

class Area : DrawingArea
{
    Color black = new Color(0, 0, 0),
          blue = new Color(0, 0, 1),
          light_green = new Color(0.56, 0.93, 0.56);

    protected override bool OnDrawn(Context c)
    {
        // draw a triangle
        c.SetSourceColor(black);
        c.LineWidth = 5;
        c.MoveTo(100, 50);
        c.LineTo(150, 150);
        c.LineTo(50, 150);
        c.ClosePath();
        c.StrokePreserve();     // draw outline, preserving path
        c.SetSourceColor(light_green);
        c.Fill();               // fill the inside

        // draw a circle
        c.SetSourceColor(blue);
        c.Arc(xc: 300, yc: 100, radius: 50, angle1: 0.0, angle2: 2 * Math.PI);
        c.Fill();

        // draw a rectangle
        c.SetSourceColor(black);
        c.Rectangle(x: 100, y: 200, width: 200, height: 100);
        c.Stroke();

        // draw text centered in the rectangle
        (int cx, int cy) = (200, 250);  // center of rectangle
        string s = "hello, cairo";
        c.SetFontSize(30);
        TextExtents te = c.TextExtents(s);
        c.MoveTo(cx - (te.Width / 2 + te.XBearing), cy - (te.Height / 2 + te.YBearing));
        c.ShowText(s);

        return true;
    }
}
