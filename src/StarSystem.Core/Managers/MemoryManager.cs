namespace StarSystem.Core
{
    /// <summary>
    /// Star System's main memory manager. Contains internal information about memory and management functions, among others.
    /// </summary>
    public static class MemoryManager
    {
        public static MemoryByte[] Storage => storage;

        private static readonly MemoryByte[] storage = new MemoryByte[StorageCapacity];
        public const int StorageCapacity = 512 * 1;

        internal static void Startup()
        {
            for (int i = 0; i < storage.Length; i++)
            {
                storage[i] = new(0, i, false);
            }
        }

        // MEMORY
        internal static MemoryPointer Malloc(int size)
        {
            int targetAddress = 0;

            // Finding Memory
            for (int i = 0; i < storage.Length; i++)
            {
                targetAddress = i;
                bool isFree = true;

                if (storage[i].IsUsed)
                {
                    continue;
                }

                for (int j = 0; j < size; j++)
                {
                    if (storage[i + j].IsUsed)
                    {
                        isFree = false;
                        break;
                    }
                }

                if (isFree)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            // Allocating Memory
            for (int i = 0; i < size; i++)
            {
                int currentAddress = targetAddress + i;
                storage[currentAddress] = new(0, storage[currentAddress].Address, true);
            }

            MemoryPointer memoryBlock = new(targetAddress, size);
            return memoryBlock;
        }
        internal static void Free(MemoryPointer memoryBlock)
        {
            for (int i = 0; i < memoryBlock.Size; i++)
            {
                storage[memoryBlock.Address + i].IsUsed = false;
            }
        }

        internal static void Write(int address, byte[] bytes)
        {
            try
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    int currentAddress = address + i;
                    SWrite(currentAddress, new(bytes[i], currentAddress, storage[currentAddress].IsUsed));
                }
            }
            catch (Exception)
            {
                SConsole.Write("[ WRITE ERROR ]", ConsoleColor.Red);
            }
        }
        internal static MemoryByte[] Read(int address, int count)
        {
            MemoryByte[] readBytes = new MemoryByte[count];

            try
            {
                for (int i = 0; i < count; i++)
                {
                    readBytes[i] = SRead(address + i);
                }
            }
            catch (Exception)
            {
                SConsole.Write("[ READ ERROR ]", ConsoleColor.Red);
            }

            return readBytes;
        }

        // SYSTEM
        private static void SWrite(int address, MemoryByte mByte)
        {
            storage[address] = mByte;
        }
        private static MemoryByte SRead(int address)
        {
            return storage[address];
        }

        // ==================== //

        /// <summary>
        /// Get all bytes of memory currently stored in the Star System.
        /// </summary>
        /// <returns>IEnumerable with all bytes of memory.</returns>
        public static IEnumerable<MemoryByte> GetStorageView()
        {
            return storage;
        }
    }

    /// <summary>
    /// Represents a byte in Star System.
    /// </summary>
    public struct MemoryByte
    {
        /// <summary>
        /// Current memory byte value.
        /// </summary>
        public byte Value { get; internal set; }

        /// <summary>
        /// Current memory byte address.
        /// </summary>
        public int Address { get; internal set; }

        /// <summary>
        /// Informs if the byte of memory is currently being used.
        /// </summary>
        public bool IsUsed { get; internal set; }

        internal MemoryByte(byte value, int address, bool isUsed)
        {
            Value = value;
            Address = address;
            IsUsed = isUsed;
        }
    }

    /// <summary>
    /// Represents a region in memory in Star System.
    /// </summary>
    public struct MemoryPointer
    {
        /// <summary>
        /// Memory address of the beginning of this memory block.
        /// </summary>
        public int Address { get; internal set; }

        /// <summary>
        /// Size of this allocated memory block.
        /// </summary>
        public int Size { get; internal set; }

        internal MemoryPointer(int address, int size)
        {
            Address = address;
            Size = size;
        }
    }
}
