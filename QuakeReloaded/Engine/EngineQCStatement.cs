using System.Runtime.InteropServices;

namespace QuakeReloaded.Engine;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct EngineQCStatement
{
    public ushort op;
    public short a, b, c;
}
