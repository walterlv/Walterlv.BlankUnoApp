using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankUnoApp;
public static class PlatformWindow
{
    [NotNull]
    public static IPlatformWindow? Current { get; set; }
}

public interface IPlatformWindow
{
    void Initialize();
}
