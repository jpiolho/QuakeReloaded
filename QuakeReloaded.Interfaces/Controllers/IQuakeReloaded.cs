﻿namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// The main interface of QuakeReloaded. Provides easy access to all the APIs.
    /// </summary>
    public interface IQuakeReloaded
    {
        /// <summary>
        /// Access to <see cref="IQuakeConsole"/>
        /// </summary>
        IQuakeConsole Console { get; }
        /// <summary>
        /// Access to <see cref="IQuakeEvents"/>
        /// </summary>
        IQuakeEvents Events { get; }
        /// <summary>
        /// Access to <see cref="IQuakeCvars"/>
        /// </summary>
        IQuakeCvars Cvars { get; }
        /// <summary>
        /// Access to <see cref="IQuakeUI"/>
        /// </summary>
        IQuakeUI UI { get; }
        /// <summary>
        /// Access to <see cref="IQuakeGame"/>
        /// </summary>
        IQuakeGame Game { get; }
        /// <summary>
        /// Access to <see cref="IQuakeClient"/>
        /// </summary>
        IQuakeClient Client { get; }
    }
}
