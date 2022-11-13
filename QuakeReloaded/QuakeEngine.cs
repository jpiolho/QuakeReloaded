using QuakeReloaded.Controllers;
using QuakeReloaded.Engine;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace QuakeReloaded
{
    internal class QuakeEngine
    {
        [Function(CallingConventions.Microsoft)]
        private delegate IntPtr FuncGetPRString(int arg1, int index);
        private FuncGetPRString _funcGetPRString;


        private unsafe EngineQCStatement** _globalPRStatements;

        internal QuakeEngine(QuakeReloadedAPI api, IReloadedHooks hooks, QuakeScanner scanner)
        {
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
}
