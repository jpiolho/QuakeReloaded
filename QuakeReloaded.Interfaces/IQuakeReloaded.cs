using System;

namespace QuakeReloaded.Interfaces
{
    public interface IQuakeReloaded
    {
        IQuakeConsole Console { get; }
        IQuakeEvents Events { get; }
        IQuakeCvars Cvars { get; }
    }
}
