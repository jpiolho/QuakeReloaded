using System;
using System.Threading.Tasks;

namespace QuakeReloaded.Interfaces
{
    public interface IQuakeEvents
    {
        [Obsolete("This API is experimental and can be removed at any time")]
        void _EXPERIMENTAL_RegisterOnInitialized(Action callback);
        [Obsolete("This API is experimental and can be removed at any time")]
        void _EXPERIMENTAL_RegisterOnRenderFrame(Action callback);
    }
}
