namespace StarSystem.Core
{
    /// <summary>
    /// Areas are special regions of the Star System that help in the construction of <see cref="SVar{T}"/> in a practical and fast way, serving only as a repository of information. You must instantiate all your variables in a class constructor.
    /// </summary>
    /// <remarks>Areas are built right after memory initialization and before building and registering commands.</remarks>
    public abstract class BaseAreaModule { }
}
