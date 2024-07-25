using FluentAssertions.Formatting;

namespace DrewSoft.Collections;

public class BucketedPriorityQueueTests
{
    [Fact]
    public void A_new_bucketed_priority_queue_is_empty()
    {
        var sut = new BucketedPriorityQueue();

        sut.Should().BeEmpty();
    }

    [Fact]
    public void A_new_bucketed_priority_queue_has_no_buckets()
    {
        var sut = new BucketedPriorityQueue();

        var result = sut.Buckets;

        result.Should().BeEmpty();
    }
}