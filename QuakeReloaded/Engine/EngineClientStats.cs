using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuakeReloaded.Engine;

[StructLayout(LayoutKind.Sequential)]
internal struct EngineClientStats
{
    public int health;
    public int frags;
    public int weapon;
    public int ammo;
    public int armor;
    public int weaponframe;
    public int shells;
    public int nails;
    public int rockets;
    public int cells;
    public int activeweapon;
    public int totalsecrets;
    public int totalmonsters;
    public int secrets;
    public int monsters;
}
