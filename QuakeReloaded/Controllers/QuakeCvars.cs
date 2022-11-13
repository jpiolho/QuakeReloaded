using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using System.Runtime.InteropServices;

namespace QuakeReloaded.Controllers;

internal class QuakeCvars : QuakeControllerBase, IQuakeCvars
{
    [Function(CallingConventions.Microsoft)]
    private delegate bool FuncCvarGetValueBool(IntPtr cvar, uint defaultValue);
    [Function(CallingConventions.Microsoft)]
    private delegate float FuncCvarGetValueFloat(IntPtr cvar, uint defaultValue);
    [Function(CallingConventions.Microsoft)]
    private delegate IntPtr FuncCvarGet(IntPtr cvarRegistry, IntPtr cvarName);
    [Function(CallingConventions.Microsoft)]
    private delegate IntPtr FuncCvarRegister(IntPtr cvarPointer, IntPtr name, IntPtr defaultValue, IntPtr description, int flags, float min, float max, bool unknown, IntPtr callback);

    private FuncCvarGet _funcCvarGet = default!;
    private FuncCvarGetValueBool _funcCvarGetValueBool = default!;
    private FuncCvarGetValueFloat _funcCvarGetValueFloat = default!;
    private FuncCvarRegister _funcCvarRegister = default!;
    private IntPtr _objCvarRegistry;
    private IntPtr _objCvarList;

    internal QuakeCvars(IQuakeReloaded api, IReloadedHooks hooks, QuakeScanner scanner) : base(api, hooks, scanner)
    {
        // Scan for cvar get bool value function
        scanner.Scan("40 57 41 56 41 57 48 83 EC 40 48 C7 44 24 ?? FE FF FF FF 48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 8B DA 48 8B F9 E8 ?? ?? ?? ?? 48 8B E8 4C 8D 3D ?? ?? ?? ?? 4C 89 7C 24 ?? 48 89 44 24 ?? 45 33 C0 48 8B D0 49 8B CF 4C 8B 0D ?? ?? ?? ?? 41 FF 51 ?? 90 8B 4F ?? 0F BA E1 08 73 ?? 8B D3 48 8B CF FF 57 ?? 48 8B C8 FF 15 ?? ?? ?? ?? 85 C0", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _funcCvarGetValueBool = hooks.CreateWrapper<FuncCvarGetValueBool>((long)offset, out var _);
        });

        // Scan for cvar registry
        scanner.Scan("48 89 35 ?? ?? ?? ?? E8 ?? ?? ?? ?? BA 01 00 00 00", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 3);
                _objCvarRegistry = mainModule.BaseAddress + result.Offset + offset + 7;
            }
        });

        // Scan for cvar get float value function
        scanner.Scan("48 8B C4 57 41 56 41 57 48 83 EC 50 48 C7 40 ?? FE FF FF FF 48 89 58 ?? 48 89 68 ?? 48 89 70 ?? 0F 29 70 ?? 8B DA", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _funcCvarGetValueFloat = hooks.CreateWrapper<FuncCvarGetValueFloat>((long)offset, out var _);
        });

        // Scan for cvar get function
        scanner.Scan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 83 79 ?? 00 48 8B EA", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _funcCvarGet = hooks.CreateWrapper<FuncCvarGet>((long)offset, out var _);
        });

        // Scan for register cvar function
        scanner.Scan("48 89 4C 24 ?? 57 48 83 EC 30 48 C7 44 24 ?? FE FF FF FF 48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 48 8B F2 48 8B D9", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _funcCvarRegister = hooks.CreateWrapper<FuncCvarRegister>((long)offset, out var _);
        });

        // Scan for register cvar list
        scanner.Scan("48 89 05 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 85 C0 74 ?? 48 89 58 ??", (mainModule, result) =>
        {
            unsafe
            {
                int offset = *(int*)(mainModule.BaseAddress + result.Offset + 3);
                _objCvarList = mainModule.BaseAddress + result.Offset + offset + 7;
            }
        });
    }

    public bool GetBoolValue(string name, bool defaultValue = false)
    {
        return GetBoolValue(GetPointer(name), defaultValue);
    }
    public bool GetBoolValue(IntPtr pointer, bool defaultValue = false)
    {
        if (pointer == IntPtr.Zero)
            return defaultValue;

        return _funcCvarGetValueBool(pointer, defaultValue ? 1u : 0u);
    }

    public IntPtr GetPointer(string name)
    {
        unsafe
        {
            using var nativeName = new NativeString(name);
            return _funcCvarGet(new IntPtr(*(void**)_objCvarRegistry), nativeName);
        }
    }

    public float GetFloatValue(string name, float defaultValue = 0)
    {
        return GetFloatValue(GetPointer(name), defaultValue);
    }

    public float GetFloatValue(IntPtr pointer, float defaultValue = 0)
    {
        const uint DefaultValuePlaceholder = 0u;
        var value = _funcCvarGetValueFloat(pointer, DefaultValuePlaceholder);
        return value == DefaultValuePlaceholder ? defaultValue : value;
    }


    public string GetStringValue(string name, string defaultValue = "") => GetStringValue(GetPointer(name), defaultValue);
    public string GetStringValue(IntPtr pointer, string defaultValue = "")
    {
        if (pointer == IntPtr.Zero)
            return defaultValue;

        unsafe
        {
            return Marshal.PtrToStringAnsi(new IntPtr(**(char***)(pointer.ToInt64() + 96))) ?? defaultValue;
        }
    }

    public bool Exists(string name) => GetPointer(name) != IntPtr.Zero;

    public IntPtr Register(string name, string defaultValue, string description = "", CvarFlags flags = CvarFlags.None, float min = 0f, float max = 1f) => _EXPERIMENTAL_Register(name, defaultValue, description, (int)flags, min, max);


    public IntPtr _EXPERIMENTAL_Register(string name, string defaultValue, string description = "", int flags = 0, float min = 0f, float max = 1f)
    {
        IntPtr namePtr = Marshal.StringToHGlobalAnsi(name);
        IntPtr defaultValuePtr = Marshal.StringToHGlobalAnsi(defaultValue);
        IntPtr descriptionPtr = Marshal.StringToHGlobalAnsi(description);
        var cvarPtr = Marshal.AllocHGlobal(140);

        _funcCvarRegister(cvarPtr, namePtr, defaultValuePtr, descriptionPtr, (int)flags, min, max, false, IntPtr.Zero);

        // Trigger a cvars list rebuild so that the cvar appears in the console
        unsafe
        {
            *(void**)(_objCvarList + 24) = null;
        }

        return cvarPtr;
    }

}
