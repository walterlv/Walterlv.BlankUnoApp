using System.Runtime.InteropServices;

namespace BlankX11App.X11;
internal static class XLib
{
    private const string libX11 = "libX11.so.6";
    private const string libX11Ext = "libXext.so.6";

    [DllImport(libX11)]
    public static extern int XDefaultScreen(nint display);

    [DllImport(libX11)]
    public static extern nint XOpenDisplay(nint display);

    [DllImport(libX11)]
    public static extern nint XRootWindow(nint display, int screen_number);

    [DllImport(libX11)]
    public static extern nint XCreateColormap(nint display, nint window, nint visual, int create);

    [DllImport(libX11)]
    public static extern nint XCreateWindow(nint display, nint parent, int x, int y, int width, int height,
        int border_width, int depth, int xclass, nint visual, nuint valuemask,
        ref XSetWindowAttributes attributes);

    [DllImport(libX11)]
    public static extern int XMapWindow(nint display, nint window);

    [DllImport(libX11)]
    public static extern int XUnmapWindow(nint display, nint window);

    [DllImport(libX11)]
    public static extern void XMatchVisualInfo(nint display, int screen, int depth, int klass, out XVisualInfo info);

    [DllImport(libX11)]
    public static extern int XMoveResizeWindow(nint display, nint window, int x, int y, int width, int height);

    [DllImport(libX11)]
    public static extern int XFlush(nint display);

    [DllImport(libX11)]
    public static extern int XDestroyWindow(nint display, nint window);

    [DllImport(libX11)]
    public static extern nint XNextEvent(nint display, out XEvent xevent);

    [DllImport(libX11Ext)]
    public static extern void XShapeCombineRectangles(
        nint display,
        nint window,
        int shape_kind,
        int x_offset,
        int y_offset,
        nint rectangles,
        int n_rectangles,
        int op,
        int ordering);
}
