using System.Collections.Immutable;

using BlankX11App.X11;

using static BlankX11App.X11.XLib;
using static BlankX11App.X11.GlxConsts;
using System.Diagnostics;
using System.Runtime.InteropServices;

//while (true)
//{
//    Thread.Sleep(100);
//    if (Debugger.IsAttached)
//    {
//        break;
//    }
//}

var display = XOpenDisplay(0);
var defaultScreen = XDefaultScreen(display);
var rootWindow = XRootWindow(display, defaultScreen);
XMatchVisualInfo(display, defaultScreen, 32, 4, out var visualInfo);
var visual = visualInfo.visual;
var valueMask = SetWindowValuemask.BackPixmap
    | SetWindowValuemask.BackPixel
    | SetWindowValuemask.BorderPixel
    | SetWindowValuemask.BitGravity
    | SetWindowValuemask.WinGravity
    | SetWindowValuemask.BackingStore
    | SetWindowValuemask.ColorMap;
var attr = new XSetWindowAttributes
{
    backing_store = 1,
    bit_gravity = Gravity.NorthWestGravity,
    win_gravity = Gravity.NorthWestGravity,
    override_redirect = 0,  // 参数：_overrideRedirect
    colormap = XCreateColormap(display, rootWindow, visual, 0),
};

var handle = XCreateWindow(display, rootWindow, 100, 100, 640, 480, 0,
    32,
    (int)CreateWindowArgs.InputOutput,
    visual,
    (nuint)valueMask, ref attr);
ClipInteraction(display, handle);
XMapWindow(display, handle);
XFlush(display);

while (XNextEvent(display, out var xEvent) == default)
{
}

XUnmapWindow(display, handle);
XDestroyWindow(display, handle);

static unsafe nint GetVisual(nint deferredDisplay, int defaultScreen)
{
    var glx = new GlxInterface();
    nint fbconfig = 0;
    XVisualInfo* visual = null;
    int[] baseAttribs =
        [
            GLX_X_RENDERABLE, 1,
            GLX_RENDER_TYPE, GLX_RGBA_BIT,
            GLX_DRAWABLE_TYPE, GLX_WINDOW_BIT | GLX_PBUFFER_BIT,
            GLX_DOUBLEBUFFER, 1,
            GLX_RED_SIZE, 8,
            GLX_GREEN_SIZE, 8,
            GLX_BLUE_SIZE, 8,
            GLX_ALPHA_SIZE, 8,
            GLX_DEPTH_SIZE, 1,
            GLX_STENCIL_SIZE, 8,
        ];

    foreach (var attribs in new[] { baseAttribs })
    {
        var ptr = glx.ChooseFBConfig(deferredDisplay, defaultScreen,
            attribs, out var count);
        for (var c = 0; c < count; c++)
        {

            var v = glx.GetVisualFromFBConfig(deferredDisplay, ptr[c]);
            // We prefer 32 bit visuals
            if (fbconfig == default || v->depth == 32)
            {
                fbconfig = ptr[c];
                visual = v;
                if (v->depth == 32)
                {
                    break;
                }
            }
        }

        if (fbconfig != default)
        {
            break;
        }
    }

    return visual->visual;
}

static void ClipInteraction(nint display, nint window)
{
    // 创建并初始化 XRectangle 结构
    XRectangle[] rectangles = new XRectangle[1];
    rectangles[0].x = 0;
    rectangles[0].y = 0;
    rectangles[0].width = 100;
    rectangles[0].height = 100;

    // 分配内存并复制矩形数组
    IntPtr unmanagedRectangles = Marshal.AllocHGlobal(Marshal.SizeOf<XRectangle>() * rectangles.Length);

    for (int i = 0; i < rectangles.Length; i++)
    {
        IntPtr currentPtr = IntPtr.Add(unmanagedRectangles, Marshal.SizeOf<XRectangle>() * i);
        Marshal.StructureToPtr(rectangles[i], currentPtr, false);
    }

    // 调用 XShapeCombineRectangles 函数
    XShapeCombineRectangles(
        display,
        window,
        0, // ShapeInput
        0, // x_offset
        0, // y_offset
        unmanagedRectangles,
        rectangles.Length,
        0, // ShapeSet
        0  // YXBanded
    );

    // 释放分配的内存
    Marshal.FreeHGlobal(unmanagedRectangles);
}
