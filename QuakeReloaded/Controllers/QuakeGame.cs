using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using System.Runtime.InteropServices;

namespace QuakeReloaded.Controllers;

internal class QuakeGame : QuakeControllerBase, IQuakeGame
{
    private unsafe char** _globalGameDir;

    internal QuakeGame(QuakeReloadedAPI api, IReloadedHooks hooks, QuakeScanner scanner) : base(api, hooks, scanner)
    {
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

    public Platform Platform => _api.Engine.Platform;

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
                return (float)_api.Engine._globalClientState->time; // This should be changed to server time
            }
        }
    }

}
