namespace DrewSoft.Collections;

public static class PriorityBucket
{
    public static PriorityBucket<T> CreateFifo<T>() 
        where T : notnull 
        => new(new(source => source));

    public static PriorityBucket<T> CreateLifo<T>() 
        where T : notnull 
        => new(new(Enumerable.Reverse));

    public static PriorityBucket<T> Create<T>(Func<IEnumerable<T>, IEnumerable<T>> ordering)
        where T : notnull  
        => new(new(ordering));
}
