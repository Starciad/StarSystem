using StarSystem.Core;

namespace StarSystem
{
    internal static class Startup
    {
        internal static void Execute()
        {
            SCore.Init(typeof(Startup).Assembly);
        }
    }
}