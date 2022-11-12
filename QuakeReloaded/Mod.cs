using Iced.Intel;
using QuakeReloaded.Configuration;
using QuakeReloaded.Controllers;
using QuakeReloaded.Interfaces;
using QuakeReloaded.Template;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.Sigscan;
using Reloaded.Memory.Sigscan.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using IReloadedHooks = Reloaded.Hooks.Definitions.IReloadedHooks;

namespace QuakeReloaded
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase, IExports // <= Do not Remove.
    {
        public Type[] GetTypes() => new[] { 
            typeof(IQuakeConsole), 
            typeof(IQuakeEvents),
            typeof(IQuakeCvars),
            typeof(IQuakeUI),
            typeof(IQuakeGame),
            typeof(IQuakeReloaded)
        };

        public override bool CanUnload() => false;
        public override bool CanSuspend() => false;

        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config _configuration;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        private class ScanResults
        {
            public int Count { get; set; }

            public IntPtr HookInitialized { get; set; }

            public IntPtr FuncConsolePrint { get; set; }
            public IntPtr ObjConsoleBuffer { get; set; }
        }

        private QuakeConsole _console;
        private QuakeCvars _cvars;
        private QuakeEvents _events;
        private QuakeUI _ui;
        private QuakeGame _game;
        private QuakeReloadedAPI _api; 

        public Mod(ModContext context)
        {
#if DEBUG
            Debugger.Launch();
#endif

            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

            CreateAndRegisterControllers();

            _events.RegisterOnInitialized(() =>
            {
                _console.PrintLine("QuakeReloaded initialized", 0, 255, 0);
            });
            _events.RegisterOnPreInitialize(() =>
            {
                _cvars.Register("qr", "1", "QuakeReloaded", CvarFlags.Integer | CvarFlags.Constant);
            });
        }


        [MemberNotNull(nameof(_events))]
        [MemberNotNull(nameof(_console))]
        [MemberNotNull(nameof(_cvars))]
        [MemberNotNull(nameof(_ui))]
        [MemberNotNull(nameof(_game))]
        private void CreateAndRegisterControllers()
        {
            var currentProcess = Process.GetCurrentProcess();
            var mainModule = currentProcess.MainModule!;

            var t = _modLoader.GetController<IStartupScanner>();
            if (!t.TryGetTarget(out var scanner))
                throw new Exception("Failed to get scanner");

            var quakeScanner = new QuakeScanner(mainModule, scanner);

            // Create controllers
            _console = new QuakeConsole(_hooks!, quakeScanner);
            _cvars = new QuakeCvars(_hooks!, quakeScanner);
            _events = new QuakeEvents(_hooks!,quakeScanner);
            _ui = new QuakeUI(_hooks!, quakeScanner);
            _game = new QuakeGame(_hooks!, quakeScanner);

            _api = new QuakeReloadedAPI()
            {
                Console = _console,
                Cvars = _cvars,
                Events = _events,
                UI = _ui,
                Game = _game
            };


            

            // Register controllers
            _modLoader.AddOrReplaceController<IQuakeConsole>(_owner, _console);
            _modLoader.AddOrReplaceController<IQuakeEvents>(_owner, _events);
            _modLoader.AddOrReplaceController<IQuakeCvars>(_owner, _cvars);
            _modLoader.AddOrReplaceController<IQuakeUI>(_owner, _ui);
            _modLoader.AddOrReplaceController<IQuakeGame>(_owner, _game);
            _modLoader.AddOrReplaceController<IQuakeReloaded>(_owner, _api);
        }

        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}