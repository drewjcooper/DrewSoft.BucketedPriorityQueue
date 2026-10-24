# DrewSoft.Collections

A .NET collection library that provides a bucketed priority queue implementation.

## Features

- Prioritised buckets with independent ordering strategies
- FIFO and LIFO bucket ordering support
- Custom ordering via `Ordering<T>` delegates
- Queue-style dequeue operations with `TryDequeue` and `Dequeue`

## Installation

```bash
dotnet add package DrewSoft.Collections
```

## Example

```csharp
using DrewSoft.Collections;

var queue = new BucketedPriorityQueue<int>
{
    new PriorityBucket<int>(Ordering<int>.Fifo) { 1, 2, 3 },
    new PriorityBucket<int>(Ordering<int>.Lifo) { 10, 20 }
};

while (queue.TryDequeue(out var item))
{
    Console.WriteLine(item);
}
```

## Custom ordering

```csharp
var queue = new BucketedPriorityQueue<string>
{
    new PriorityBucket<string>(new Ordering<string>(source => source.OrderBy(x => x.Length)))
    {
        "three", "six", "nine"
    }
};
```

## License

MIT
