using System.Collections;

namespace Assignment.Data.Interfaces;

/// <summary>
/// Collection Interface
/// </summary>
/// <typeparam name="T">
///The param T is needed as a generic, when the class gets implemented you specify the exact class it needs to be and
/// all the functions change the return value to that class
/// </typeparam>
public interface IObjectCollection<T> : IEnumerable<T>
{

#pragma warning disable CS1591
    T GetById(int id);
    List<T> Cache { get; }
    bool IsCacheUsed { get; set; }
    T Add(T item);
    void Update(T item);
    void Delete(int id);

    /// <summary>
    /// Returns the class enumerator which will be connected to a Cache or _cache  field
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}