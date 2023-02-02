namespace StarSystem.Core
{
    public static class SVarPool
    {
        private static readonly List<SVar> tempVars = new();

        public static SVar Add(SVar value)
        {
            tempVars.Add(value);
            return value;
        }

        internal static void Instantiate()
        {
            Destroy();
        }
        internal static void Destroy()
        {
            tempVars.ForEach(x => x.Dispose());
            tempVars.Clear();
        }
    }
}
