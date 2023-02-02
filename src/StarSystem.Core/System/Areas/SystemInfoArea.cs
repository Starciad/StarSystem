namespace StarSystem.Core
{
    public sealed class SystemInfoArea : BaseAreaModule
    {
        public string Username
        {
            get => SUsername.Read();

            set => SUsername.Write(value);
        }
        public Version Version => SVersion.Read();

        internal SVar<string> SUsername { get; set; }
        internal SVar<Version> SVersion { get; set; }

        public SystemInfoArea()
        {
            SUsername = new("User");
            SVersion = new(new(0, 0, 1));
        }
    }
}
