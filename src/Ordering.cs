namespace DrewSoft.Collections;

public class Ordering<T>(Func<IEnumerable<T>, IEnumerable<T>> ordering)
{
    public static readonly Ordering<T> Fifo = new(source => source);
    public static readonly Ordering<T> Lifo = new(Enumerable.Reverse);

    private readonly Func<IEnumerable<T>, IEnumerable<T>> _ordering = ordering;

    public IEnumerable<T> Apply(IEnumerable<T> source) => _ordering.Invoke(source);
}