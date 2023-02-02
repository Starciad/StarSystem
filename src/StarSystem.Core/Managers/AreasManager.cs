namespace StarSystem.Core
{
    /// <summary>
    /// Class that manages areas registered in the Star System and allows the capture of information among other functionalities.
    /// </summary>
    public static class AreasManager
    {
        private static readonly List<BaseAreaModule> registeredSystemModules = new();

        internal static void Startup()
        {
            List<Type> commandModuleTypes = new();

            commandModuleTypes.AddRange(typeof(CommandManager).Assembly.GetTypes());
            commandModuleTypes.AddRange(SCore.InstanceAssembly.GetTypes());

            foreach (Type commandModuleType in commandModuleTypes.Where(x => x.IsSubclassOf(typeof(BaseAreaModule))))
            {
                registeredSystemModules.Add((BaseAreaModule)Activator.CreateInstance(commandModuleType));
            }
        }

        /// <summary>
        /// Obtains, through a generic type, a specific area registered in the Star System.
        /// </summary>
        /// <typeparam name="T">The type of area requested to search.</typeparam>
        /// <returns>Returns the requested area.</returns>
        public static T GetModule<T>() where T : BaseAreaModule
        {
            return registeredSystemModules.OfType<T>().FirstOrDefault();
        }
    }
}
