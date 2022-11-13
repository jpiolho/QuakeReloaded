using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using System.Runtime.InteropServices;

namespace QuakeReloaded.Controllers;

internal class QuakeGame : QuakeControllerBase, IQuakeGame
{
    private unsafe double* _globalMapTime;
    private unsafe char** _globalGameDir;

    internal QuakeGame(IQuakeReloaded api, IReloadedHooks hooks, QuakeScanner scanner) : base(api, hooks, scanner)
    {
        // Scan for global map time
        scanner.Scan("F2 0F 10 15 ?? ?? ?? ?? 0F 28 C2 F2 0F 5E 05 ?? ?? ?? ?? F2 0F 2C D8 6B C3 3C 66 0F 6E C8 F3 0F E6 C9 F2 0F 5C D1 F2 0F 2C FA E8 ?? ?? ?? ??", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 4);
                _globalMapTime = (double*)(mainModule.BaseAddress + result.Offset + offset + 8);
            }
        });

        // Scan for global game dir
        scanner.Scan("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 41 B9 D1 00 00 00", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 3);
                _globalGameDir = (char**)(mainModule.BaseAddress + result.Offset + offset + 7);
            }
        });
    }

    public string ModPath
    {
        get
        {
            unsafe
            {
                return Marshal.PtrToStringUTF8(new nint(*_globalGameDir)) ?? string.Empty;
            }
        }
    }

    public string Mod
    {
        get
        {
            return Path.GetDirectoryName(ModPath) ?? string.Empty;
        }
    }

    public float MapTime
    {
        get
        {
            unsafe
            {
                return (float)*_globalMapTime;
            }
        }
    }
}
