using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;

namespace QuakeReloaded.Controllers;

class QuakeEvents : IQuakeEvents
{
    [Function(CallingConventions.Microsoft)]
    private delegate void HookInitializeQuake();
    private IHook<HookInitializeQuake> _hookInitializeQuake;
    private List<Action> _eventOnInitialized = new();

    [Function(CallingConventions.Microsoft)]
    private delegate void HookRenderFrame(IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4);
    private IHook<HookRenderFrame> _hookRenderFrame;
    private List<Action> _eventOnRenderFrame = new();

    public QuakeEvents(IReloadedHooks hooks, QuakeScanner scanner)
    {
        // Scan for initialize hook
        scanner.Scan("48 8B C4 55 41 54 41 55 41 56 41 57 48 8D 68 ?? 48 81 EC D0 00 00 00 48 C7 45 ?? FE FF FF FF 48 89 58 ?? 48 89 70 ?? 48 89 78 ?? 45 33 ED", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _hookInitializeQuake = hooks.CreateHook<HookInitializeQuake>(HookInitializeQuakeHandler, (long)offset).Activate();
        });

        // Scan for render frame hook
        scanner.Scan("48 8B C4 55 53 41 56 48 8D 68 ??", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _hookRenderFrame = hooks.CreateHook<HookRenderFrame>(HookRenderFrameHandler, (long)offset).Activate();
        });
    }

    private void HookInitializeQuakeHandler()
    {
        _hookInitializeQuake.OriginalFunction.Invoke();
        RaiseOnInitialized();
    }

    private void HookRenderFrameHandler(IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4)
    {
        RaiseOnRenderFrame();
        _hookRenderFrame.OriginalFunction.Invoke(arg1, arg2, arg3, arg4);
    }

    public void _EXPERIMENTAL_RegisterOnInitialized(Action callback) => _eventOnInitialized.Add(callback);
    public void _EXPERIMENTAL_RegisterOnRenderFrame(Action callback) => _eventOnRenderFrame.Add(callback);


    public void RaiseOnInitialized()
    {
        foreach (var callback in _eventOnInitialized)
            callback();
    }

    public void RaiseOnRenderFrame()
    {
        foreach (var callback in _eventOnRenderFrame)
            callback();
    }
}
