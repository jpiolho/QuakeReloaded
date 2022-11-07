using System;
using System.Threading.Tasks;

namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// Handles registration of events from the game to the mods.
    /// </summary>
    public interface IQuakeEvents
    {
        /// <summary>
        /// Event called very early, right when the engine is starting.
        /// </summary>
        IQuakeCallbackReference RegisterOnPreInitialize(Action callback);
        /// <summary>
        /// Event called after all the game systems have been initialized.
        /// </summary>
        IQuakeCallbackReference RegisterOnInitialized(Action callback);
        /// <summary>
        /// Event called on every frame.
        /// </summary>
        IQuakeCallbackReference RegisterOnRenderFrame(Action callback);
    }
}
