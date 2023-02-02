namespace StarSystem.Core
{
    internal sealed class MemoryCommands : BaseCommandModule
    {
        [Command("StorageView")]
        [Aliases("SView", "StorageV", "SV")]
        [Description("View information related to system memory.")]
        public static void StorageViewCommand()
        {
            IEnumerable<MemoryByte> storage = MemoryManager.GetStorageView();

            SConsole.WriteLine("[ Storage Information ]", ConsoleColor.Cyan);
            Console.WriteLine($"> Total Storage: {storage.Count()} Bytes;");
            Console.WriteLine($"> Storage Used: {storage.Where(x => x.IsUsed).Count()} Bytes;");
            Console.WriteLine($"> Remaining Storage: {storage.Where(x => !x.IsUsed).Count()} Bytes;\n");

            SConsole.WriteLine("[ VIEW ]", ConsoleColor.Cyan);
            foreach (MemoryByte mByte in storage)
            {
                if (mByte.IsUsed)
                {
                    SConsole.Write($"{mByte.Value} ", ConsoleColor.Yellow);
                }
                else
                {
                    SConsole.Write($"{mByte.Value} ", ConsoleColor.Red);
                }
            }
        }

        [Command("StorageDelete")]
        [Aliases("SDelete", "StorageD", "SD")]
        [Description("Delete all storage information.")]
        public static void StorageDeleteCommand()
        {
            SConsole.WriteLine("Are you sure about this? (y/n)", ConsoleColor.Red);

            try
            {
                switch (SConsole.GetNextInput<char>().ToString().ToLower())
                {
                    case "y":
                        Clear();
                        return;

                    case "n":
                        NotClear();
                        return;
                }
            }
            catch (Exception)
            {
                NotClear();
                return;
            }

            void Clear()
            {
                int storageLength = MemoryManager.Storage.Length;
                for (int i = 0; i < storageLength; i++)
                {
                    MemoryManager.Storage[i].Value = 0;
                    MemoryManager.Storage[i].IsUsed = false;
                }

                SConsole.Write("All allocated bytes have been freed!", ConsoleColor.Yellow);
            }

            void NotClear()
            {
                SConsole.Write("The decision was reversed.", ConsoleColor.Yellow);
            }
        }
    }
}