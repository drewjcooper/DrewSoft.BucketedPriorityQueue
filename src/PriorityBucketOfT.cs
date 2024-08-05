using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DrewSoft.Collections;

public class PriorityBucket<T>(Ordering<T>? bucketOrder = null) : ICollection<T>
    where T: notnull
{
    private readonly List<T> _items = [];
    private Ordering<T> _bucketOrder = bucketOrder ?? Ordering<T>.Fifo;

    public int Count => _items.Count;

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(T item) => _items.Add(item);

    internal bool TryDequeue([NotNullWhen(true)] out T? item)
    {
        if (_items.Count > 0)
        {
            item = _bucketOrder.Apply(_items).First();
            _items.Remove(item);
            return true;
        }

        item = default;
        return false;
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(T item)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator() => _bucketOrder.Apply(_items).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
