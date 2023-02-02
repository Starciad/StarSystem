namespace StarSystem.Core
{
    internal sealed class UserCommands : BaseCommandModule
    {
        [Command("GetUsername")]
        [Aliases("GUsername", "GUser", "GetU", "GU")]
        [Description("Shows the current name of the user on the system.")]
        public static void GetCurrentUserCommand()
        {
            SConsole.Write(AreasManager.GetModule<SystemInfoArea>().Username, ConsoleColor.Yellow);
        }

        [Command("SetUsername")]
        [Aliases("SUsername", "SUser", "SetU", "SU")]
        [Description("Updates the current username on the system.")]
        public static void SetCurrentUserCommand([ReadAllText] string username)
        {
            AreasManager.GetModule<SystemInfoArea>().Username = username;
            SConsole.Write($"Name updated to: {AreasManager.GetModule<SystemInfoArea>().Username}", ConsoleColor.Yellow);
        }
    }
}