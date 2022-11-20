using QuakeReloaded.Interfaces;

namespace QuakeReloaded.Controllers
{
    internal class QuakeReloadedAPI : IQuakeReloaded
    {
        public IQuakeEvents Events { get; internal set; } = default!;

        public IQuakeCvars Cvars { get; internal set; } = default!;

        public IQuakeConsole Console { get; internal set; } = default!;
        public IQuakeUI UI { get; internal set; } = default!;
        public IQuakeGame Game { get; internal set; } = default!;
        public IQuakeClient Client { get; set; } = default!;


        internal QuakeEngine Engine { get; set; } = default!;
    }
}
