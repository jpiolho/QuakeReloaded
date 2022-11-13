using QuakeReloaded.Engine;
using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;

namespace QuakeReloaded.Controllers;

class QuakeEvents : QuakeControllerBase, IQuakeEvents
{
    [Function(CallingConventions.Microsoft)]
    private delegate void HookInitializeQuake();
    private IHook<HookInitializeQuake> _hookInitializeQuake;
    private List<Action> _eventOnInitialized = new();

    [Function(CallingConventions.Microsoft)]
    private delegate void HookPreInitializeQuake(IntPtr arg1);
    private IHook<HookPreInitializeQuake> _hookPreInitializeQuake;
    private List<Action> _eventOnPreInitialize = new();

    [Function(CallingConventions.Microsoft)]
    private delegate void HookRenderFrame(IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4);
    private IHook<HookRenderFrame> _hookRenderFrame;
    private List<Action> _eventOnRenderFrame = new();

    [Function(CallingConventions.Microsoft)]
    private delegate int HookQCFunction(IntPtr arg1);
    private IHook<HookQCFunction> _hookQCFunction;
    private Dictionary<string, List<Func<EventHandling>>> _eventOnQCFunction = new();




    internal QuakeEvents(QuakeReloadedAPI api, IReloadedHooks hooks, QuakeScanner scanner) : base(api, hooks, scanner)
    {
        // Scan for initialize hook
        scanner.Scan("48 8B C4 55 41 54 41 55 41 56 41 57 48 8D 68 ?? 48 81 EC D0 00 00 00 48 C7 45 ?? FE FF FF FF 48 89 58 ?? 48 89 70 ?? 48 89 78 ?? 45 33 ED", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _hookInitializeQuake = hooks.CreateHook<HookInitializeQuake>(HookInitializeQuakeHandler, (long)offset).Activate();
        });

        // Scan for pre-initialize hook
        scanner.Scan("48 8B C4 55 41 54 41 55 41 56 41 57 48 8B EC 48 81 EC 80 00 00 00 48 C7 45 ?? FE FF FF FF 48 89 58 ?? 48 89 70 ?? 48 89 78 ?? 4C 8B E9", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _hookPreInitializeQuake = hooks.CreateHook<HookPreInitializeQuake>(HookPreInitializeQuakeHandler, (long)offset).Activate();
        });

        // Scan for render frame hook
        scanner.Scan("48 8B C4 55 53 41 56 48 8D 68 ??", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _hookRenderFrame = hooks.CreateHook<HookRenderFrame>(HookRenderFrameHandler, (long)offset).Activate();
        });

        // Scan for QC function hook
        scanner.Scan("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 4C 63 05 ?? ?? ?? ??", (mainModule, result) =>
        {
            var offset = mainModule.BaseAddress + result.Offset;
            _hookQCFunction = hooks.CreateHook<HookQCFunction>(HookQCFunctionHandler, (long)offset).Activate();
        });

        
    }

    private void HookInitializeQuakeHandler()
    {
        _hookInitializeQuake.OriginalFunction.Invoke();
        RaiseOnInitialized();
    }

    private void HookPreInitializeQuakeHandler(IntPtr arg1)
    {
        RaiseOnPreInitialize();
        _hookPreInitializeQuake.OriginalFunction.Invoke(arg1);
    }

    private void HookRenderFrameHandler(IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4)
    {
        RaiseOnRenderFrame();
        _hookRenderFrame.OriginalFunction.Invoke(arg1, arg2, arg3, arg4);
    }

    private int HookQCFunctionHandler(IntPtr arg1)
    {
        bool superceded = false;

        unsafe {
            var functionNamePtr = *(int*)(arg1 + 16);
            var functionName = _api.Engine.GetPRString(functionNamePtr);

            if (functionName != null) {
                if(_eventOnQCFunction.TryGetValue(functionName, out var hooks))
                {
                    foreach(var hook in hooks)
                    {
                        var handling = hook();

                        // Find a return symbol
                        if (handling == EventHandling.Superceded)
                        {
                            superceded = true;
                            break;
                        }
                    }
                }
            }

            var idx = _hookQCFunction.OriginalFunction.Invoke(arg1);

            // If superceded, trick the engine to go to a "RET" opcode
            if(superceded)
                return EngineUtils.QCGetReturnStatementIndex(_api.Engine);

            return idx;
        }
    }

    public IQuakeCallbackReference RegisterOnInitialized(Action callback)
    {
        _eventOnInitialized.Add(callback);
        return new QuakeCallbackReference(() => _eventOnInitialized.Remove(callback));
    }
    public IQuakeCallbackReference RegisterOnRenderFrame(Action callback)
    {
        _eventOnRenderFrame.Add(callback);
        return new QuakeCallbackReference(() => _eventOnRenderFrame.Remove(callback));
    }
    public IQuakeCallbackReference RegisterOnPreInitialize(Action callback)
    {
        _eventOnPreInitialize.Add(callback);
        return new QuakeCallbackReference(() => _eventOnPreInitialize.Remove(callback));
    }

    public IQuakeCallbackReference RegisterHandlerQCFunction(string functionName, Func<EventHandling> callback)
    {
        if (!_eventOnQCFunction.TryGetValue(functionName, out var list))
            _eventOnQCFunction[functionName] = list = new List<Func<EventHandling>>();

        list.Add(callback);
        return new QuakeCallbackReference(() => _eventOnQCFunction[functionName]?.Remove(callback));
    }

    public void RaiseOnPreInitialize()
    {
        foreach (var callback in _eventOnPreInitialize)
            callback();
    }

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



    private void HandleCallbacks<TCallback>(IList<WeakReference<TCallback>> list, Action<TCallback> handler) where TCallback : class
    {
        var toDeleteIndexes = new List<int>();

        for (var i = 0; i < list.Count; i++)
        {
            if (!list[i].TryGetTarget(out var callback))
            {
                toDeleteIndexes.Add(i);
                continue;
            }

            handler(callback);
        }
    }
}
