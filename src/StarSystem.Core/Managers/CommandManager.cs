namespace StarSystem.Core
{
    /// <summary>
    /// Registered system general command manager. See and get information among other features.
    /// </summary>
    public static class CommandManager
    {
        /// <summary>
        /// IEnumerable of all commands registered on the system.
        /// </summary>
        public static IEnumerable<BaseCommandModule> RegisteredModules => registeredModules;
        private static readonly List<BaseCommandModule> registeredModules = new();

        internal static void Startup()
        {
            RegisterModules();
            BuildModules();
        }

        private static void RegisterModules()
        {
            List<Type> commandTypes = new();

            commandTypes.AddRange(typeof(CommandManager).Assembly.GetTypes());
            commandTypes.AddRange(SCore.InstanceAssembly.GetTypes());

            foreach (Type commandModuleType in commandTypes.Where(x => x.IsSubclassOf(typeof(BaseCommandModule))))
            {
                registeredModules.Add((BaseCommandModule)Activator.CreateInstance(commandModuleType));
            }
        }
        private static void BuildModules()
        {
            List<Task> tasks = new();
            foreach (BaseCommandModule commandModule in registeredModules)
            {
                tasks.Add(Task.Run(commandModule.Build));
            }

            Task.WaitAll(tasks.ToArray());
        }

        // =================== //

        internal static void Execute(string[] commandTokens)
        {
            string commandName = commandTokens[0];
            string[] commandArgs = new string[commandTokens.Length - 1];

            Array.Copy(commandTokens, 1, commandArgs, 0, commandArgs.Length);

            Command command = null;
            foreach (BaseCommandModule commandModule in registeredModules)
            {
                command = commandModule.GetCommand(commandName);
                if (command != null)
                {
                    break;
                }
            }

            if (command == null)
            {
                Console.Write("Command Not Exists!");
                return;
            }

            SVarPool.Instantiate();
            command.Invoke(commandArgs);
            SVarPool.Destroy();
        }
    }
}
