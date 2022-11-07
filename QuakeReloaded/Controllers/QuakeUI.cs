using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using System.Runtime.InteropServices;
using System;

namespace QuakeReloaded.Controllers;

internal class QuakeUI : IQuakeUI
{
    private unsafe int* _resolutionWidth, _resolutionHeight;

    private unsafe void** _globalFont, _globalUIContext;
    private unsafe float* _globalFontSize;

    private IntPtr _drawTextPositionArray;

    [Function(CallingConventions.Microsoft)]
    private delegate void FuncDrawText(IntPtr font, IntPtr arg2, IntPtr text, IntPtr position, int size, int alignment, IntPtr color);
    private FuncDrawText _funcDrawText = default!;

    public QuakeUI(IReloadedHooks hooks, QuakeScanner scanner)
    {
        // Scan for function to draw text
        scanner.Scan("48 89 5C 24 ?? 48 89 74 24 ?? 48 89 7C 24 ?? 55 48 8B EC 48 83 EC 60 41 80 38 00 49 8B F9 48 8B F2 48 8B D9 0F 84 ?? ?? ?? ?? 49 C7 C1 FF FF FF FF 49 8B D0 E8 ?? ?? ?? ?? F3 0F 10 07 8B 45 ??", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _funcDrawText = hooks.CreateWrapper<FuncDrawText>((long)offset, out var _);
        });

        // Get global font object
        scanner.Scan("48 89 05 ?? ?? ?? ?? 48 83 B8 ?? ?? ?? ?? 00", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 3);
                _globalFont = (void**)(mainModule.BaseAddress + result.Offset + offset + 7);
            }
        });

        // Get global font size object
        scanner.Scan("E8 ?? ?? ?? ?? F3 0F 11 05 ?? ?? ?? ?? 45 0F 57 DB", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 5 + 4);
                _globalFontSize = (float*)(mainModule.BaseAddress + result.Offset + offset + 5 + 8);
            }
        });
        

        // Get global ui context
        scanner.Scan("48 89 05 ?? ?? ?? ?? 41 BE 01 00 00 00 4C 8B 6D ??", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 3);
                _globalUIContext = (void**)(mainModule.BaseAddress + result.Offset + offset + 7);
            }
        });
        

        // Scan for resolution globals
        scanner.Scan("44 8B 05 ?? ?? ?? ?? 45 03 C1 45 03 C7 8B 0D ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 03 D1 89 4C 24 ?? 44 89 4C 24 ?? 89 54 24 ?? 44 89 44 24 ?? 48 8B 03", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 3);
                _resolutionHeight = (int*)(mainModule.BaseAddress + result.Offset + offset + 7);

                offset = *(int*)(mainModule.BaseAddress + result.Offset + 19 + 2);
                _resolutionWidth = (int*)(mainModule.BaseAddress + (result.Offset + 19) + offset + 6);
            }
        });


        _drawTextPositionArray = Marshal.AllocHGlobal(sizeof(float) * 2);
    }

    public int ResolutionWidth
    {
        get
        {
            unsafe { return *_resolutionWidth; }
        }
    }

    public int ResolutionHeight
    {
        get
        {
            unsafe { return *_resolutionHeight; }
        }
    }

    public void DrawText(string text, float x, float y)
    {
        DrawText(text, (int)(ResolutionWidth * x), (int)(ResolutionHeight * y));
    }

    public void DrawText(string text, int x, int y)
    {
        unsafe
        {
            using var ns = new NativeString(text);

            var position = (float*)_drawTextPositionArray.ToPointer();
            position[0] = (float)x;
            position[1] = (float)y;

            float size = *_globalFontSize;
            uint color = 0xFFFFFFFF;

            _funcDrawText.Invoke(new IntPtr(*_globalFont), new IntPtr(*_globalUIContext), ns, _drawTextPositionArray, *(int*)&size, 0, new IntPtr(&color));
        }
    }
}
