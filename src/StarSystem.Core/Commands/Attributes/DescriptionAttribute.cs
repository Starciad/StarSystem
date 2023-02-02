namespace StarSystem.Core
{
    /// <summary>
    /// Base attribute for adding/creating descriptions for your commands.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute
    {
        internal string Content { get; private set; }

        /// <summary>
        /// Initializes the description for a command.
        /// </summary>
        /// <param name="content">The content of the description for the command.</param>
        public DescriptionAttribute(string content)
        {
            Content = content;
        }
    }
}
