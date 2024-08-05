using System.Runtime.InteropServices;
using FluentAssertions.Formatting;

namespace DrewSoft.Collections;

public class BucketedPriorityQueueTests
{
    [Fact]
    public void A_new_bucketed_priority_queue_is_empty()
    {
        var sut = new BucketedPriorityQueue<int>();

        sut.Should().BeEmpty();
    }

    [Fact]
    public void A_new_bucketed_priority_queue_has_no_buckets()
    {
        var sut = new BucketedPriorityQueue<int>();

        var result = sut.Buckets;

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Dequeueing_an_item_from_an_empty_queue_throws_invalid_operation(bool hasBuckets)
    {
        var sut = new BucketedPriorityQueue<int>();
        if (hasBuckets) { sut.Add([]); }

        var act = () => _ = sut.Dequeue();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot dequeue an item from an empty queue.");
    }

    [Fact]
    public void Adding_a_bucket_to_a_new_queue_adds_it_to_the_buckets_collection()
    {
        var sut = new BucketedPriorityQueue<int>();
        var bucket = new PriorityBucket<int>();

        sut.Add(bucket);

        sut.Buckets.Should().BeEquivalentTo(new[] { bucket });
    }

    [Fact]
    public void Adding_a_bucket_with_an_item_to_a_new_queue_makes_the_item_available_to_dequeue()
    {
        var item = 42;
        var sut = new BucketedPriorityQueue<int> { ([item]) };

        var result = sut.Dequeue();

        result.Should().Be(item);
    }

    [Fact]
    public void Dequeueing_an_item_removes_it_from_its_bucket()
    {
        var bucket = new PriorityBucket<int> { 23 };
        var sut = new BucketedPriorityQueue<int> { bucket };

        bucket.Should().HaveCount(1);

        sut.Dequeue();

        bucket.Should().HaveCount(0);
    }

    [Theory]
    [MemberData(nameof(QueueEnumerationTestCases))]
    public void A_queue_enumerates_items_in_the_order_determined_by_the_prioritised_buckets(
        IEnumerable<PriorityBucket<string>> buckets, 
        IEnumerable<string> expected)
    {
        var sut = new BucketedPriorityQueue<string>(buckets);

        var result = sut.AsEnumerable();

        result.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }

    public static TheoryData<IEnumerable<PriorityBucket<string>>, IEnumerable<string>> QueueEnumerationTestCases =>
        new()
        {
            // Single buckets
            { [ [ "2", "4", "3", "1", "5" ] ], [ "2", "4", "3", "1", "5" ] },
            { [ new(Ordering<string>.Lifo) { "2", "4", "3", "1", "5" } ], [ "5", "1", "3", "4", "2" ] },
            { 
                [ new(new(s => s.OrderByDescending(e => e.Length))) { "one", "two", "three", "four", "five" } ], 
                [ "three", "four", "five", "one", "two" ] 
            },
            // Multiple ordered buckets
            {
                [
                    new(Ordering<string>.Lifo) { "x", "y", "z" },
                    new(new(s => s.OrderBy(e => e.Length))) { "three", "six", "nine" },
                    new(Ordering<string>.Fifo) { "2", "4", "3" } 

                ],
                [ "z", "y", "x", "six", "nine", "three", "2", "4", "3" ]
            }
        };

    [Theory]
    [MemberData(nameof(OrderedBucketInsertionTestCases))]
    public void Inserting_an_ordered_bucket_inserts_it_in_priority_order(int beforeIndex, IEnumerable<string> expected)
    {
        var sut = new BucketedPriorityQueue<string> 
        { 
            new(Ordering<string>.Lifo) { "x", "y", "z" },
            new(Ordering<string>.Fifo) { "2", "4", "3" } 
        };
        var newBucket = new PriorityBucket<string>(new(s => s.OrderBy(e => e.Length))) { "three", "six", "nine" };

        sut.Insert(beforeIndex, newBucket);

        sut.Should().BeEquivalentTo(expected, opts => opts.WithStrictOrdering());
    }

    public static TheoryData<int, IEnumerable<string>> OrderedBucketInsertionTestCases =>
        new()
        {
            { 0, [ "six", "nine", "three", "z", "y", "x", "2", "4", "3" ] },
            { 1, [ "z", "y", "x", "six", "nine", "three", "2", "4", "3" ] },
            { 5, [ "z", "y", "x", "2", "4", "3", "six", "nine", "three" ] }
        };

    [Fact]
    public void Repeated_dequeuing_returns_items_in_enumerator_order_across_buckets()
    {
        var sut = new BucketedPriorityQueue<int>
        {
            new PriorityBucket<int>(Ordering<int>.Fifo) { 1, 2 },
            new PriorityBucket<int>(Ordering<int>.Lifo) { 3, 4 }
        };

        var result = new[] { sut.Dequeue(), sut.Dequeue(), sut.Dequeue(), sut.Dequeue() };

        result.Should().BeEquivalentTo([1, 2, 4, 3], opts => opts.WithStrictOrdering());
    }

    [Fact]
    public void TryDequeue_on_an_empty_queue_returns_false()
    {
        var sut = new BucketedPriorityQueue<int>();

        var result = sut.TryDequeue(out var item);

        result.Should().BeFalse();
    }

    [Fact]
    public void TryDequeue_with_queue_items_returns_true_and_the_first_item()
    {
        var sut = new BucketedPriorityQueue<int> { new PriorityBucket<int> { 1, 2 } };

        var result = sut.TryDequeue(out var item);

        result.Should().BeTrue();
        item.Should().Be(1);
    }
}