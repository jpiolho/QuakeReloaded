using System;
using System.Threading.Tasks;

namespace QuakeReloaded.Interfaces
{
    public interface IQuakeEvents
    {
        IQuakeCallbackReference RegisterOnPreInitialize(Action callback);
        IQuakeCallbackReference RegisterOnInitialized(Action callback);
        IQuakeCallbackReference RegisterOnRenderFrame(Action callback);
    }
}
