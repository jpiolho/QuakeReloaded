using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;

namespace QuakeReloaded.Controllers;

internal class QuakeControllerBase
{
    protected IQuakeReloaded _api;

    public QuakeControllerBase(IQuakeReloaded api, IReloadedHooks hooks, QuakeScanner scanner)
    {
        _api = api;
    }
}
