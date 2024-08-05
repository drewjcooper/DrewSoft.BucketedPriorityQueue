using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace DrewSoft.Collections;

public class BucketedPriorityQueue<T> : IEnumerable<T>
    where T : notnull
{
    private readonly List<PriorityBucket<T>> _buckets;

    public BucketedPriorityQueue() => _buckets = [];

    public BucketedPriorityQueue(IEnumerable<PriorityBucket<T>> buckets) => _buckets = [..buckets];

    public IEnumerable<PriorityBucket<T>> Buckets => _buckets.AsReadOnly();

    public void Add(PriorityBucket<T> bucket) => _buckets.Add(bucket);

    public void Insert(int index, PriorityBucket<T> bucket)
    {
        if (index < _buckets.Count) 
        {
            _buckets.Insert(index, bucket);
        }
        else
        {
            _buckets.Add(bucket);
        }
    }

    public T Dequeue()
        => TryDequeue(out var item)
            ? item
            : throw new InvalidOperationException("Cannot dequeue an item from an empty queue.");

    public bool TryDequeue([NotNullWhen(true)] out T? item)
    {
        foreach (var bucket in _buckets)
        {
            if (bucket.TryDequeue(out item)) { return true; }
        }

        item = default;
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _buckets.SelectMany(x => x).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
