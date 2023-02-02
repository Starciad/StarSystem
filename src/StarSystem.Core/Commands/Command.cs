using System.Reflection;

namespace StarSystem.Core
{
    /// <summary>
    /// A command that is executed internally by the Star System upon certain events.
    /// </summary>
    /// <remarks>
    /// Commands can be created from public methods with the <see cref="CommandAttribute"/> in classes derived from <see cref="BaseCommandModule"/>.
    /// </remarks>
    public class Command
    {
        /// <summary>
        /// The name of this command.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The aliases for this command.
        /// </summary>
        public string[] Aliases { get; private set; }

        /// <summary>
        /// The description of this command.
        /// </summary>
        public string Description { get; private set; }

        internal List<MethodInfo> Actions { get; set; }
        private BaseCommandModule Module { get; set; }

        internal Command(CommandAttribute command, AliasesAttribute aliases, DescriptionAttribute description, BaseCommandModule module)
        {
            Name = command.Name;
            Aliases = aliases.Aliases ?? null;
            Description = description.Content ?? null;
            Module = module;
            Actions = new();
        }

        internal void Invoke(string[] args)
        {
            List<object> objectArgs = new();
            MethodInfo target = null!;

            // CHECK ACTIONS SETTINGS
            foreach (MethodInfo action in Actions)
            {
                target = action;
                
                // CHECK PARAMS
                ParameterInfo[] currentActionParamsInfos = action.GetParameters();
                if (currentActionParamsInfos.Length == 0 && args.Length == 0)
                    break;

                for (int i = 0; i < currentActionParamsInfos.Length; i++)
                {
                    ParameterInfo paramInfo = currentActionParamsInfos[i];

                    try
                    {
                        if (paramInfo.GetCustomAttribute<ReadAllTextAttribute>() != null)
                        {
                            objectArgs.Add(MakeTextWithArray(args, args.Length - i, i));
                            break;
                        }

                        objectArgs.Add(Convert.ChangeType(args[i], paramInfo.ParameterType));
                    }
                    catch (Exception)
                    {
                        objectArgs.Clear();
                        continue;
                    }
                }
            }

            // COMMAND ERROR
            if (target == null)
            {
                SConsole.Write("[ COMMAND ERROR ]", ConsoleColor.Red);
                return;
            }

            // COMMAND INVOKING
            try
            {
                _ = target.Invoke(Module, (objectArgs.Count == 0 ? null : objectArgs.ToArray()));
            }
            catch (Exception)
            {
                SConsole.Write("[ COMMAND INVOKE ERROR ]", ConsoleColor.Red);
                return;
            }
        }

        internal void AddAction(MethodInfo action)
        {
            Actions.Add(action);
        }

        // ================= //
        // UTILITIES
        private static string MakeTextWithArray(string[] textArray, int length, int position)
        {
            string[] textArrayString = new string[length];
            Array.Copy(textArray, position, textArrayString, 0, length);

            return string.Join(' ', textArrayString);
        }
    }
}
