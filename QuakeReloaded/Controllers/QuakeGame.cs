using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;

namespace QuakeReloaded.Controllers;

internal class QuakeGame : IQuakeGame
{
    private unsafe double* _globalMapTime;

    internal QuakeGame(IReloadedHooks hooks, QuakeScanner scanner)
    {
        // Scan
        scanner.Scan("F2 0F 10 15 ?? ?? ?? ?? 0F 28 C2 F2 0F 5E 05 ?? ?? ?? ?? F2 0F 2C D8 6B C3 3C 66 0F 6E C8 F3 0F E6 C9 F2 0F 5C D1 F2 0F 2C FA E8 ?? ?? ?? ??", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 4);
                _globalMapTime = (double*)(mainModule.BaseAddress + result.Offset + offset + 8);
            }
        });
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
