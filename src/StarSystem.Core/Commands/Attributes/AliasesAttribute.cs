namespace StarSystem.Core
{
    /// <summary>
    /// Base attribute responsible for creating a collection of aliases for a command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AliasesAttribute : Attribute
    {
        internal string[] Aliases { get; private set; }

        /// <summary>
        /// Initialize an attribute with an alias collection.
        /// </summary>
        /// <param name="aliases">Collection of aliases for this command.</param>
        public AliasesAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}