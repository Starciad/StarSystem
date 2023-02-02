using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

namespace StarSystem.Core
{
    public abstract class SVar : IDisposable
    {
        /// <summary>
        /// Gets whether the current variable is read-only.
        /// </summary>
        public bool IsReadOnly { get; internal set; }
        internal MemoryPointer MemoryPointer { get; set; }

        /// <summary>
        /// Frees the variable value that was allocated in the Star System.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            MemoryManager.Free(MemoryPointer);

            GC.Collect(GC.GetGeneration(this), GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }
    }

    /// <summary>
    /// Virtual variable that uses/stores resources internally in Star System.
    /// </summary>
    /// <typeparam name="T">The type of object that will be stored internally in the system.</typeparam>
    public class SVar<T> : SVar
    {
        /// <summary>
        /// Initializes the variable with a default value allocated in Star System.
        /// </summary>
        public SVar() 
        {
            Write(default(T));
        }

        /// <summary>
        /// Initializes the variable with a value allocated in Star System.
        /// </summary>
        /// <param name="value">The initial value of the variable that will be allocated in memory upon its initialization.</param>
        public SVar(T value)
        {
            Write(value);
        }

        /// <summary>
        /// Initializes the variable with a value allocated in Star System.
        /// </summary>
        /// <param name="value">The initial value of the variable that will be allocated in memory upon its initialization.</param>
        /// <param name="readOnly">Defines whether the variable is read-only. If so, the <see cref="Write(T)"/> method will not work.</param>
        public SVar(T value, bool readOnly)
        {
            Write(value);
            IsReadOnly = readOnly;
        }

        /// <summary>
        /// Write a new value that will replace the old value of this variable. By doing so, this method frees and reallocates space on the Star System.
        /// </summary>
        /// <remarks>If the variable is read-only, this method does not work.</remarks>
        /// <param name="value">Value that will overwrite the old value.</param>
        public void Write(T value)
        {
            if (IsReadOnly)
                return;

            // GET
            byte[] byteValues = JsonSerializer.SerializeToUtf8Bytes(value);

            // FREE & MALLOC
            MemoryManager.Free(MemoryPointer);
            MemoryPointer = MemoryManager.Malloc(byteValues.Length);

            // WRITE
            MemoryManager.Write(MemoryPointer.Address, byteValues);
        }

        /// <summary>
        /// Read the contents of this variable allocated in the Star System.
        /// </summary>
        /// <returns>Returns the last allocated value in the system.</returns>
        public T Read()
        {
            try
            {
                MemoryByte[] bytes = MemoryManager.Read(MemoryPointer.Address, MemoryPointer.Size);
                byte[] memoryBytes = bytes.Select(x => x.Value).ToArray();

                return (T)JsonSerializer.Deserialize(Encoding.UTF8.GetString(memoryBytes).ToArray(), typeof(T));
            }
            catch (Exception)
            {
                SConsole.Write("Could not find the value of the variable in memory.", ConsoleColor.Red);
                return default(T);
            }
        }
    }
}
