using System;
using System.Collections.Generic;
using System.Text;

namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// Provides access to the game console.
    /// </summary>
    public interface IQuakeConsole
    {
        /// <summary>
        /// Prints partial text to the console. The text color will be white.
        /// Note that partial text will not be shown until a line is finished either by printing a '\n' character or calling <see cref="PrintLine(string)"/>.
        /// </summary>
        /// <param name="text">The text that will be printed to the console.</param>
        void Print(string text);
        /// <summary>
        /// Prints partial text to the console with the specified color.
        /// Note that partial text will not be shown until a line is finished either by printing a '\n' character or calling <see cref="PrintLine(string)"/>.
        /// </summary>
        /// <param name="text">The text that will be printed to the console.</param>
        /// <param name="colorABGR">The color of the text as an unsigned integer in ABGR format. Example full blue: 0xFFFF0000</param>
        void Print(string text, uint colorABGR);
        /// <summary>
        /// Prints partial text to the console with the specified color.
        /// Note that partial text will not be shown until a line is finished either by printing a '\n' character or calling <see cref="PrintLine(string)"/>.
        /// </summary>
        /// <param name="text">The text that will be printed to the console.</param>
        /// <param name="r">The red component of the text color. 0: No red, 255: Full red</param>
        /// <param name="g">The green component of the text color. 0: No green, 255: Full green</param>
        /// <param name="b">The blue component of the text color. 0: No blue, 255: Full blue</param>
        /// <param name="a">The alpha component of the text color. 0: Transparent, 255: Fully opaque</param>
        void Print(string text, byte r, byte g, byte b, byte a = 255);

        /// <summary>
        /// Prints a text line to the console. The text color will be white.
        /// </summary>
        /// <param name="text">The message that will be printed to the console</param>
        void PrintLine(string text);
        /// <summary>
        /// Prints a text line to the console.
        /// </summary>
        /// <param name="text">The message that will be printed to the console</param>
        /// <param name="colorABGR">The color of the text as an unsigned integer in ABGR format. Example full blue: 0xFFFF0000</param>
        void PrintLine(string text, uint colorABGR);
        /// <summary>
        /// Prints a text line to the console.
        /// </summary>
        /// <param name="text">The message that will be printed to the console</param>
        /// <param name="r">The red component of the text color. 0: No red, 255: Full red</param>
        /// <param name="g">The green component of the text color. 0: No green, 255: Full green</param>
        /// <param name="b">The blue component of the text color. 0: No blue, 255: Full blue</param>
        /// <param name="a">The alpha component of the text color. 0: Transparent, 255: Fully opaque</param>
        void PrintLine(string text, byte r, byte g, byte b, byte a = 255);
    }
}
