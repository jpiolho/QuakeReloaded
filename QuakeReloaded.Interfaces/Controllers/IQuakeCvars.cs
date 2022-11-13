using System;

namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// Provides access to the game cvars.
    /// </summary>
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
        /// <summary>
        /// Gets a boolean value from a cvar using a cvar pointer.
        /// </summary>
        /// <param name="pointer">Pointer to the cvar</param>
        /// <param name="defaultValue">The default value of the cvar if it isn't set, value is invalid or doesn't exist.</param>
        /// <returns>The value of the cvar. 0 = false, 1 = true</returns>
        bool GetBoolValue(IntPtr pointer, bool defaultValue = false);

        /// <summary>
        /// Gets a float value from a cvar.
        /// </summary>
        /// <param name="name">Name of the cvar</param>
        /// <param name="defaultValue">The default value for the cvar if it isn't set, value is invalid or doesn't exist.</param>
        /// <returns>The value of the cvar</returns>
        float GetFloatValue(string name, float defaultValue = 0.0f);
        /// <summary>
        /// Gets a float value from a cvar.
        /// </summary>
        /// <param name="pointer">Pointer to the cvar</param>
        /// <param name="defaultValue">The default value for the cvar if it isn't set, value is invalid or doesn't exist.</param>
        /// <returns>The value of the cvar, as a <see cref="float"/></returns>
        float GetFloatValue(IntPtr pointer, float defaultValue = 0.0f);

        /// <summary>
        /// Gets a string value from a cvar.
        /// </summary>
        /// <param name="pointer">Pointer to the cvar</param>
        /// <param name="defaultValue">The default value for the cvar if it isn't set, value is invalid or doesn't exist.</param>
        /// <returns>The value of the cvar, as a <see cref="string"/></returns>
        string GetStringValue(IntPtr pointer, string defaultValue = "");
        /// <summary>
        /// Gets a string value from a cvar.
        /// </summary>
        /// <param name="name">Name of the cvar</param>
        /// <param name="defaultValue">The default value for the cvar if it isn't set, value is invalid or doesn't exist.</param>
        /// <returns>The value of the cvar, as a <see cref="string"/></returns>
        string GetStringValue(string name, string defaultValue = "");

        /// <summary>
        /// Gets an <see cref="int"/> value from a cvar.
        /// </summary>
        /// <param name="name">Name of the cvar</param>
        /// <param name="defaultValue">The default value for the cvar if it isn't set, value is invalid or doesn't exist.</param>
        /// <returns>The value of the cvar, as a <see cref="float"/></returns>
        int GetIntValue(string name, int defaultValue = 0);
        /// <summary>
        /// Gets an <see cref="int"/> value from a cvar.
        /// </summary>
        /// <param name="pointer">Pointer to the cvar</param>
        /// <param name="defaultValue">The default value for the cvar if it isn't set, value is invalid or doesn't exist.</param>
        /// <returns>The value of the cvar, as a <see cref="float"/></returns>
        int GetIntValue(IntPtr pointer, int defaultValue = 0);

        /// <summary>
        /// Registers a new cvar. If you're using <see cref="CvarFlags.Saved"/> make sure to register it in PreInitialize.
        /// </summary>
        /// <param name="name">Name of the cvar</param>
        /// <param name="defaultValue">The initial value of the cvar</param>
        /// <param name="description">Description that shows in the console</param>
        /// <param name="flags">Attributes of the cvar</param>
        /// <param name="min">Minimum cvar value</param>
        /// <param name="max">Maximum cvar value</param>
        /// <returns>Pointer to the created cvar</returns>
        IntPtr Register(string name, string defaultValue, string description = "", CvarFlags flags = CvarFlags.None, float min = 0f, float max = 1f);

        #region Experimental
        [Obsolete("This API is experimental and can be removed at any time")]
        IntPtr _EXPERIMENTAL_Register(string name, string defaultValue, string description = "", int flags = 0, float min = 0f, float max = 1f);

        #endregion
    }
}
