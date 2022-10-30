using System;
using System.Collections.Generic;
using System.Text;

namespace QuakeReloaded.Interfaces
{
    public interface IQuakeCvars
    {
        /// <summary>
        /// Gets a pointer to the cvar object. This pointer can then be used with other cvar methods in order to optimize speed.
        /// </summary>
        /// <param name="name">Name of the cvar</param>
        /// <returns>Pointer to the cvar object. If the cvar doesn't exist, <see cref="IntPtr.Zero"/> is returned instead.</returns>
        IntPtr GetPointer(string name);

        /// <summary>
        /// Returns whether a specific cvar exists.
        /// </summary>
        /// <param name="name">Name of the cvar</param>
        /// <returns>True if a cvar with the specified <paramref name="name"/> exists, false otherwise.</returns>
        bool Exists(string name);

        /// <summary>
        /// Gets a boolean value from a cvar.
        /// </summary>
        /// <param name="name">Name of the cvar</param>
        /// <param name="defaultValue">The default value of the cvar if it isn't set, value is invalid or doesn't exist.</param>
        /// <returns>The value of the cvar. 0 = false, 1 = true</returns>
        bool GetBoolValue(string name, bool defaultValue = false);
        bool GetBoolValue(IntPtr pointer, bool defaultValue = false);

        float GetFloatValue(string name, float defaultValue = 0.0f);
        float GetFloatValue(IntPtr pointer, float defaultValue = 0.0f);

        IntPtr Register(string name, string defaultValue, string description = "", CvarFlags flags = CvarFlags.None, float min = 0f, float max = 1f);

        #region Experimental
        [Obsolete("This API is experimental and can be removed at any time")]
        IntPtr _EXPERIMENTAL_Register(string name, string defaultValue, string description = "", int flags = 0, float min = 0f, float max = 1f);

        #endregion
    }
}
