using System.Reflection;

namespace StarSystem.Core
{
    /// <summary>
    /// Command modules are special Star System classes that serve as a repository of commands that are registered and used by the system.
    /// </summary>
    /// <remarks>
    /// For building commands in this module, use the <see cref="CommandAttribute"/> attribute on your public methods, and add <see cref="DescriptionAttribute"/> and <see cref="AliasesAttribute"/> (optional) for more detailed information about your command.
    /// </remarks>
    public abstract class BaseCommandModule
    {
        internal IEnumerable<Command> RegisteredCommands => registeredCommands;
        private readonly List<Command> registeredCommands = new();

        internal void Build()
        {
            OnAwake();
            RegisterCommands();
            OnStart();
        }
        private void RegisterCommands()
        {
            foreach (MethodInfo methodInfo in GetType().GetRuntimeMethods())
            {
                CommandAttribute commandAttribute = methodInfo.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute == null) // This method not is a Command.
                {
                    continue;
                }

                Command commandTarget = registeredCommands.Find(x => x.Name == commandAttribute.Name);
                if (commandTarget == null) // Command not exists.
                {
                    AliasesAttribute aliasesAttribute = methodInfo.GetCustomAttribute<AliasesAttribute>();
                    DescriptionAttribute descriptionAttribute = methodInfo.GetCustomAttribute<DescriptionAttribute>();

                    commandTarget = new(commandAttribute, aliasesAttribute, descriptionAttribute, this);
                    commandTarget.AddAction(methodInfo);

                    registeredCommands.Add(commandTarget);
                }
                else // Command exists.
                {
                    commandTarget.AddAction(methodInfo);
                }
            }
        }

        /// <summary>
        /// Method invoked before building and registering the commands of this module.
        /// </summary>
        protected virtual void OnAwake() { return; }

        /// <summary>
        /// Method invoked right after the construction and registration of all commands in this module.
        /// </summary>
        protected virtual void OnStart() { return; }

        // UTILITIES
        internal Command GetCommand(string name)
        {
            foreach (Command command in registeredCommands)
            {
                if (command.Name == name || Array.Find(command.Aliases, x => x == name) != null)
                {
                    return command;
                }
            }

            return null;
        }
    }
}
