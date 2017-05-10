using System.Collections.Generic;

namespace BExpansion.Collections
{
    /// <summary>
    /// Represents a strongly typed list of objects that can be indexed by index. The list appears to repeat infinitely
    /// (e.g. for a list of length 2, the zero-based index '2' will return the first value).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LoopedList<T> : List<T>
    {
        public new T this[int index] => base[index % base.Count];
    }
}
