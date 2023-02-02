namespace StarSystem.Core
{
    /// <summary>
    /// Base attribute for creating Star System commands.
    /// </summary>
    /// <remarks>
    /// To create a command, add this attribute to a public method of a class derived from <see cref="BaseCommandModule"/>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        internal string Name { get; private set; }

        /// <summary>
        /// Initialize a system command.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        public CommandAttribute(string name)
        {
            Name = name;
        }
    }
}
