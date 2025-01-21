namespace Collections.Tests.DataStructure;

public class QueueTest
{
    private readonly Collections.DataStructure.Lists.Queue<string> _queue;

    public QueueTest()
    {
        _queue = new Collections.DataStructure.Lists.Queue<string>();
        _queue.Enqueue("aaa");
        _queue.Enqueue("bbb");
        _queue.Enqueue("ccc");
        _queue.Enqueue("ddd");
        _queue.Enqueue("eee");
        _queue.Enqueue("fff");
        _queue.Enqueue("ggg");
    }

    [Fact]
    public void WhenEnqueue_ThenAddTheItemToTheQueue()
    {
        // Arrange
        // Act
        _queue.Enqueue("hhh");
        // Assert
        Assert.Multiple(
            () =>
            {
                Assert.Equal(8, _queue.Count);
                Assert.Equal(8, _queue.ToArray().Length);
            });
    }

    [Fact]
    public void WhenDequeue_ThenRemoveTheItemFromTheQueue()
    {
        // Arrange
        // Act
        _queue.Enqueue("hhh");
        _queue.Dequeue();
        _queue.Dequeue();
        var top = _queue.Dequeue();
        _queue.Dequeue();
        _queue.Dequeue();
        // Assert
        Assert.Multiple(
            () =>
            {
                Assert.Equal("ccc", top);
                Assert.Equal("fff", _queue.Top);
                Assert.Equal(3, _queue.Count);
            });
    }
}