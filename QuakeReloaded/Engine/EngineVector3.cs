﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuakeReloaded.Engine;

[StructLayout(LayoutKind.Sequential)]
internal struct EngineVector3
{
    public float x, y, z;

    public Vector3 ToVector3() => new Vector3(x, y, z);
    public static EngineVector3 FromVector3(Vector3 v) => new EngineVector3() { x = v.X, y = v.Y, z = v.Z };
}
