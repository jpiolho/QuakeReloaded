namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// Indicates how the event was handled
    /// </summary>
    public enum EventHandling
    {
        /// <summary>
        /// Nothing was done, just continue with the event as normal.
        /// </summary>
        Continue = 0,
        /// <summary>
        /// The event was handled, but let the event carry on.
        /// </summary>
        Handled = 1,
        /// <summary>
        /// The event was handled and block further execution of it.
        /// </summary>
        Superceded = 2
    }
}
