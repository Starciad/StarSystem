using System.Reflection;

namespace StarSystem.Core
{
    public static class SCore
    {
        internal static Assembly InstanceAssembly { get; private set; }

        public static void Init(Assembly assembly)
        {
            InstanceAssembly = assembly;

            BuildComponents();
            StartupSystem();
        }

        private static void BuildComponents()
        {
            MemoryManager.Startup();
            AreasManager.Startup();
            CommandManager.Startup();
        }

        private static void StartupSystem()
        {
            Console.ResetColor();
            Console.WriteLine($"{new string('=', 32)}\n");
            Console.WriteLine($"Welcome to Star System!\n");
            Console.Write($"{new string('=', 32)}");

            while (true)
            {
                Console.Write("\n\n>>> ");
                string[] commandTokens = Console.ReadLine().Split(' ');
                CommandManager.Execute(commandTokens);
            }
        }
    }
}
