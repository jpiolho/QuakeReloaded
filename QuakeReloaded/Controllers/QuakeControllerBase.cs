using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeReloaded.Controllers;

internal class QuakeControllerBase
{
    protected IQuakeReloaded _api;

    public QuakeControllerBase(IQuakeReloaded api,IReloadedHooks hooks, QuakeScanner scanner)
    {
        _api = api;
    }
}
