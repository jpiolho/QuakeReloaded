using QuakeReloaded.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuakeReloaded.Controllers
{
    internal class QuakeReloadedAPI : IQuakeReloaded
    {
        public IQuakeEvents Events { get; internal set; } = default!;

        public IQuakeCvars Cvars { get; internal set; } = default!;

        public IQuakeConsole Console { get; internal set; } = default!;
    }
}
