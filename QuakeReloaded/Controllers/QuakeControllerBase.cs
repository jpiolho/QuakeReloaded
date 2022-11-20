using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;

namespace QuakeReloaded.Controllers;

internal class QuakeControllerBase
{
    protected QuakeReloadedAPI _api;

    public QuakeControllerBase(QuakeReloadedAPI api, IReloadedHooks hooks, QuakeScanner scanner)
    {
        _api = api;
    }
}
