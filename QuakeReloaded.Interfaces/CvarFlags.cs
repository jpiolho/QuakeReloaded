using System;
using System.Collections.Generic;
using System.Text;

namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// Specifies the property flags of a cvar
    /// </summary>
    [Flags]
    public enum CvarFlags : int
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,
        
        /// <summary>
        /// The cvar type is boolean. It can only be 0 or 1.
        /// </summary>
        Boolean = 0x1,
        /// <summary>
        /// The cvar type is integer. It'll only allow integer values.
        /// </summary>
        Integer = 0x2,
        /// <summary>
        /// The cvar type is float.
        /// </summary>
        Float = 0x4,
        /// <summary>
        /// The cvar type is string.
        /// </summary>
        String = 0x8,

        /// <summary>
        /// The cvar will be saved in the user config
        /// </summary>
        Saved = 0x10,

        /// <summary>
        /// The cvar is a constant value and cannot be changed
        /// </summary>
        Constant = 0x8000
    }
}
