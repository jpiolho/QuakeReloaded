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
    }
}
