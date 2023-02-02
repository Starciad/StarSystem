namespace StarSystem.Core
{
    /// <summary>
    /// Console with some extra utilities and functions for writing on the console.
    /// </summary>
    public static class SConsole
    {
        public static void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public static void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Write(string value, ConsoleColor color)
        {
            SetColor(color);
            Console.Write(value);
            ResetColor();
        }

        public static void WriteLine(string value, ConsoleColor color)
        {
            SetColor(color);
            Console.WriteLine(value);
            ResetColor();
        }

        public static T GetNextInput<T>()
        {
            Console.Write(SSystemInternal.CONSOLE_INPUT_SYMBOL);

            T value = (T)Convert.ChangeType(Console.ReadLine() ?? string.Empty, typeof(T));
            return value;
        }
    }
}
