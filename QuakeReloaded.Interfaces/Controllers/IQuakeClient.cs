using System.Numerics;

namespace QuakeReloaded.Interfaces
{
    /// <summary>
    /// Provides access to client-only related things
    /// </summary>
    public interface IQuakeClient
    {
        /// <summary>
        /// Gets how much health the UI should show
        /// </summary>
        int Health { get; }

        /// <summary>
        /// Gets the items the player has
        /// </summary>
        int Items { get; }

        /// <summary>
        /// Gets how many frags the UI should show
        /// </summary>
        int Frags { get; }

        /// <summary>
        /// Gets how many rockets the UI should show
        /// </summary>
        int Rockets { get; }

        /// <summary>
        /// Gets how many shells the UI should show
        /// </summary>
        int Shells { get; }

        /// <summary>
        /// Gets how many nails the UI should show
        /// </summary>
        int Nails { get; }

        /// <summary>
        /// Gets how much armor the UI should show
        /// </summary>
        int Armor { get; }

        /// <summary>
        /// Gets how much game time the UI should show
        /// </summary>
        float Time { get; }

        /// <summary>
        /// Gets if the UI should show intermission or not
        /// </summary>
        bool InIntermission { get; }

        /// <summary>
        /// Gets how much time should be shown in the intermission
        /// </summary>
        float IntermissionTime { get; }

        /// <summary>
        /// Gets if the client is inside water
        /// </summary>
        bool InWater { get; }

        /// <summary>
        /// Gets if the client is on the floor
        /// </summary>
        bool OnGround { get; }

        /// <summary>
        /// Which level name should be shown to the client
        /// </summary>
        string LevelName { get; }

        /// <summary>
        /// What's the view height from origin
        /// </summary>
        float ViewHeight { get; }

        /// <summary>
        /// The angles indicating the direction that the player is looking at
        /// </summary>
        Vector3 ViewAngles { get; }

        /// <summary>
        /// How fast the client is moving
        /// </summary>
        Vector3 Velocity { get; }
    }
}
