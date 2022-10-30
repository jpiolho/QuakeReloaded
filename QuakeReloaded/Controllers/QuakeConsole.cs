using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;

namespace QuakeReloaded.Controllers;

public class QuakeConsole : IQuakeConsole
{
    [Function(CallingConventions.Microsoft)]
    private delegate void FuncConsolePrint(IntPtr buffer, IntPtr color, IntPtr message);

    private FuncConsolePrint _funcConsolePrint = default!;
    private IntPtr _objConsoleBuffer;


    internal QuakeConsole(IReloadedHooks hooks, QuakeScanner scanner)
    {
        // Scan console print function
        scanner.Scan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 41 54 41 55 41 56 41 57 48 81 EC 20 02 00 00 49 8B E8", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _funcConsolePrint = hooks.CreateWrapper<FuncConsolePrint>((long)offset, out var _);
        });

        // Scan for console buffer address
        scanner.Scan("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 43 ??", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 3);
                _objConsoleBuffer = mainModule.BaseAddress + result.Offset + offset + 7;
            }
        });
    }

    public void Print(string message)
    {
        Print(message, 0xFFFFFFFF);
    }
    public void Print(string message, byte r, byte g, byte b, byte a = 255)
    {
        Print(message, (uint)(a << 24 | b << 16 | g << 8 | r));
    }

    public void Print(string message, uint colorABGR)
    {
        using var nativeMessage = new NativeString(message);

        unsafe
        {
            _funcConsolePrint.Invoke(_objConsoleBuffer, new IntPtr(&colorABGR), nativeMessage);
        }
    }

    public void PrintLine(string message) => Print($"{message}\n", 0xFFFFFFFF);
    public void PrintLine(string message, byte r, byte g, byte b, byte a = 255) => Print($"{message}\n", r, g, b, a);
    public void PrintLine(string message, uint colorABGR) => Print($"{message}\n", colorABGR);

}
