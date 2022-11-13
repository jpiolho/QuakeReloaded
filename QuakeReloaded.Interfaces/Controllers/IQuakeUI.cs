namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// Provides things that are related to the UI of the game. Affects the client only.
    /// </summary>
    public interface IQuakeUI
    {
        /// <summary>
        /// Gets the client horizontal resolution
        /// </summary>
        int ResolutionWidth { get; }
        /// <summary>
        /// Gets the client vertical resolution
        /// </summary>
        int ResolutionHeight { get; }

        /// <summary>
        /// Draws a string to the screen of the client.
        /// </summary>
        /// <param name="text">The text that'll be drawn</param>
        /// <param name="x">Relative horizontal position of the text on the screen</param>
        /// <param name="y">Relative vertical position of the text on the screen.</param>
        void DrawText(string text, float x, float y);

        /// <summary>
        /// Draws a string to the screen of the client.
        /// </summary>
        /// <param name="text">The text that'll be drawn</param>
        /// <param name="x">Absolute horizontal position of the text on the screen</param>
        /// <param name="y">Absolute vertical position of the text on the screen.</param>
        void DrawText(string text, int x, int y);
    }
}
