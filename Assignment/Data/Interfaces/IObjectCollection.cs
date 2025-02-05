using System.Collections;
using Assignment.Data.Models;

namespace Assignment.Data.Interfaces;

public interface IObjectCollection<T> : IEnumerable<T>
{
    T GetById(int id);
    List<Customers> Cache { get;}
    bool IsCacheUsed { get; set; }
    T Add(T customer);
    void Update(T customer);
    void Delete(int id);



    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }
    

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}