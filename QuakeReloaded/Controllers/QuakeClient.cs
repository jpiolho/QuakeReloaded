using QuakeReloaded.Interfaces;
using QuakeReloaded.Utilities;
using Reloaded.Hooks.Definitions;
using System.Numerics;
using System.Runtime.InteropServices;

namespace QuakeReloaded.Controllers;

internal class QuakeClient : QuakeControllerBase, IQuakeClient
{
    internal QuakeClient(QuakeReloadedAPI api, IReloadedHooks hooks, QuakeScanner scanner) : base(api, hooks, scanner)
    {
    }

    public int Health { get { unsafe { return _api.Engine._globalClientState->stats.health; } } }
    public int Armor { get { unsafe { return _api.Engine._globalClientState->stats.armor; } } }
    public int Frags { get { unsafe { return _api.Engine._globalClientState->stats.frags; } } }
    public int Items { get { unsafe { return _api.Engine._globalClientState->items; } } }
    public int Shells { get { unsafe { return _api.Engine._globalClientState->stats.shells; } } }
    public int Rockets { get { unsafe { return _api.Engine._globalClientState->stats.rockets; } } }
    public int Cells { get { unsafe { return _api.Engine._globalClientState->stats.cells; } } }
    public int Nails { get { unsafe { return _api.Engine._globalClientState->stats.nails; } } }
    
    public float Time { get { unsafe { return (float)_api.Engine._globalClientState->time; } } }
    
    public bool InIntermission { get { unsafe { return _api.Engine._globalClientState->intermission; } } }
    public float IntermissionTime { get { unsafe { return (float)_api.Engine._globalClientState->intermissionTime; } } }

    public bool InWater { get { unsafe { return _api.Engine._globalClientState->inWater; } } }
    public bool OnGround { get { unsafe { return _api.Engine._globalClientState->onGround; } } }

    public Vector3 Velocity { get { unsafe { return _api.Engine._globalClientState->velocity.ToVector3(); } } }
    public Vector3 ViewAngles { get { unsafe { return _api.Engine._globalClientState->viewAngles.ToVector3(); } } }
    public float ViewHeight { get { unsafe { return _api.Engine._globalClientState->viewHeight; } } }

    public string? LevelName { get { unsafe { return Marshal.PtrToStringUTF8(_api.Engine._globalClientState->levelname); } } }
    
}
