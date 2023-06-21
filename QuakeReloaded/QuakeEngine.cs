using QuakeReloaded.Controllers;
using QuakeReloaded.Engine;
using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using System.Runtime.InteropServices;

namespace QuakeReloaded;

internal class QuakeEngine
{
    [Function(CallingConventions.Microsoft)]
    private delegate IntPtr FuncGetPRString(int arg1, int index);
    private FuncGetPRString _funcGetPRString;


    private unsafe EngineQCStatement** _globalPRStatements;

    public unsafe EngineClientState* _globalClientState;
    public Platform Platform { get; private set; }

    internal QuakeEngine(QuakeReloadedAPI api, IReloadedHooks hooks, QuakeScanner scanner)
    {
        // Load which platform the game is running on
        Platform = Win32Utils.GetImageExportName(scanner.MainModule.BaseAddress) switch
        {
            string x when x.Equals("bastet_Shipping_Playfab_Steam_x64.exe", StringComparison.InvariantCultureIgnoreCase) => Platform.Steam,
            string x when x.Equals("bastet_Shipping_Playfab_GOG_x64.exe", StringComparison.CurrentCultureIgnoreCase) => Platform.GOG,
            _ => Platform.Unknown
        };


        scanner.Scan("48 83 EC 28 48 63 CA", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _funcGetPRString = hooks.CreateWrapper<FuncGetPRString>((long)offset, out var _);
        });

        scanner.Scan("48 8B 05 ?? ?? ?? ?? 48 8D 0C ?? E8 ?? ?? ?? ?? E8 ?? ?? ?? ??", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 3);
                _globalPRStatements = (EngineQCStatement**)(mainModule.BaseAddress + result.Offset + offset + 7);
            }
        });


        scanner.Scan("8B 05 ?? ?? ?? ?? FF C0 89 05 ?? ?? ?? ?? 83 F8 02 0F 8E ?? ?? ?? ??", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 2);
                _globalClientState = (EngineClientState*)(mainModule.BaseAddress + result.Offset + offset + 6);
            }
        });

    }

    public unsafe EngineQCStatement* QCGetStatement(int index)
    {
        return &(*_globalPRStatements)[index];
    }

    public unsafe string? GetPRString(int index)
    {
        if (index == 0)
            return null;

        IntPtr str = _funcGetPRString.Invoke(0, index);
        return Marshal.PtrToStringUTF8(str);
    }
}
