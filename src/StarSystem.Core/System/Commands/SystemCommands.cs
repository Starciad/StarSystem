namespace StarSystem.Core
{
    internal sealed class SystemCommands : BaseCommandModule
    {
        [Command("Version")]
        [Aliases("V")]
        [Description("View the current system version.")]
        public static void VersionCommand()
        {
            SConsole.Write(AreasManager.GetModule<SystemInfoArea>().Version?.ToString(), ConsoleColor.Yellow);
        }

        [Command("Clear")]
        [Aliases("C")]
        [Description("Clear the console.")]
        public static void ClearCommand()
        {
            Console.Clear();
        }

        [Command("Exit")]
        [Aliases("Quit", "E", "Q")]
        [Description("Exit the console.")]
        public static void ExitCommand()
        {
            Environment.Exit(0);
        }
    }
}