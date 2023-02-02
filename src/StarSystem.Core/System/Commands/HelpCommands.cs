using System.Reflection;

namespace StarSystem.Core
{
    internal sealed class HelpCommands : BaseCommandModule
    {
        [Command("Help")]
        [Aliases("H")]
        [Description("Get help on commands.")]
        public static void HelpCommand()
        {
            string spacingString = new(' ', 4);

            SConsole.WriteLine("[ Welcome to Help! ]", ConsoleColor.Cyan);
            Console.WriteLine("To receive help on a certain command, just type:");
            SConsole.WriteLine("- Help <command_name>", ConsoleColor.Yellow);

            SConsole.WriteLine("\n[ Commands Registered ]", ConsoleColor.Cyan);
            foreach (BaseCommandModule commandModule in CommandManager.RegisteredModules)
            {
                Console.WriteLine($"{commandModule.GetType().Name}");
                foreach (Command command in commandModule.RegisteredCommands)
                {
                    Console.WriteLine($"{spacingString}- {command.Name}");
                }
                Console.WriteLine();
            }
        }

        [Command("Help")]
        public static void HelpCommand(string commandName)
        {
            Command commandTarget = null;
            foreach (BaseCommandModule commandModule in CommandManager.RegisteredModules)
            {
                commandTarget = commandModule.GetCommand(commandName);
                if (commandTarget != null)
                    break;
            }

            if (commandTarget == null)
            {
                SConsole.Write("Command not Found!", ConsoleColor.Red);
                return;
            }

            // COMAND FOUND
            SConsole.WriteLine($"[ Command Help! ]\n", ConsoleColor.Green);

            // Name
            SConsole.Write($"Name: ", ConsoleColor.Yellow);
            Console.WriteLine(commandTarget.Name);

            // Aliases
            SConsole.Write($"Aliases: ", ConsoleColor.Yellow);
            int aliasesCount = commandTarget.Aliases.Length;
            for (int i = 0; i < aliasesCount; i++)
            {
                string aliase = commandTarget.Aliases[i];
                Console.Write($"{(i < aliasesCount - 1 ? $"{aliase}," : $"{aliase}" )} ");
            }

            // Description
            SConsole.Write($"\nDescription: ", ConsoleColor.Yellow);
            Console.WriteLine(commandTarget.Description);

            // Params
            SConsole.WriteLine($"\n[ Params ]", ConsoleColor.Yellow);
            foreach (MethodInfo action in commandTarget.Actions)
            {
                Console.Write($"- {commandTarget.Name} ");
                foreach (ParameterInfo parameter in action.GetParameters())
                {
                    Console.Write($"<{parameter.Name}> ");
                }

                Console.WriteLine();
            }
        }
    }
}
