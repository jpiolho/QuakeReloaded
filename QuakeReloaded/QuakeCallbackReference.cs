using QuakeReloaded.Interfaces;

namespace QuakeReloaded;

internal class QuakeCallbackReference : IQuakeCallbackReference
{
    private Action? _deregistration;

    internal QuakeCallbackReference(Action? deregistration = null)
    {
        _deregistration = deregistration;
    }

    public void Deregister()
    {
        _deregistration?.Invoke();
    }
}
