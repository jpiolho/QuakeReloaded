namespace QuakeReloaded.Interfaces
{
    public interface IQuakeCallbackReference
    {
        /// <summary>
        /// Deregisters this callback from the events, so it won't be called anymore
        /// </summary>
        void Deregister();
    }
}
