namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// Provides access to general things from the game.
    /// </summary>
    public interface IQuakeGame
    {
        /// <summary>
        /// Returns the currently elapsed time in the map, from the client perspective.
        /// </summary>
        float MapTime { get; }

        /// <summary>
        /// Returns the full path to the current 'game'
        /// </summary>
        string ModPath { get; }

        /// <summary>
        /// Returns the current 'game' that's active. Eg: id1, ctf, hipnotic
        /// </summary>
        string Mod { get; }

        /// <summary>
        /// Returns the platform for which the game was compiled for
        /// </summary>
        Platform Platform { get; }
    }
}
