using System.Runtime.InteropServices;

namespace QuakeReloaded.Engine;

[StructLayout(LayoutKind.Explicit)]
internal struct EngineClientState
{
    [FieldOffset(32)] public EngineClientStats stats;
    [FieldOffset(160)] public int items;
    [FieldOffset(448)] public EngineVector3 viewAngles;
    [FieldOffset(484)] public EngineVector3 velocity;
    [FieldOffset(496)] public EngineVector3 punchAngle;
    [FieldOffset(552)] public float viewHeight;
    [FieldOffset(560)] public bool onGround;
    [FieldOffset(561)] public bool inWater;
    [FieldOffset(4364)] public bool intermission;
    [FieldOffset(4368)] public double intermissionTime;
    [FieldOffset(4392)] public double time;
    [FieldOffset(37184)] public IntPtr levelname;
    [FieldOffset(37312)] public int maxplayers;
    [FieldOffset(37316)] public int gametype;
    [FieldOffset(37336)] public int numEntities;
    [FieldOffset(37340)] public int numStatics;
    [FieldOffset(37344)] public int cdTrack;
    [FieldOffset(37348)] public int loopTrack;
    [FieldOffset(37352)] public IntPtr scoreboard;
}
