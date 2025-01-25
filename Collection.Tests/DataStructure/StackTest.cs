namespace Collections.Tests.DataStructure;

public class StackTest
{
    private readonly Collections.DataStructure.List.Stack<int> _stack;

    public StackTest()
    {
        _stack = new Collections.DataStructure.List.Stack<int>();
        _stack.Push(1);
        _stack.Push(2);
        _stack.Push(3);
        _stack.Push(4);
        _stack.Push(5);
    }

    [Fact]
    public void DoTest()
    {
        // Arrange
        _stack.Push(6);

        // Act
        var array = _stack.ToArray();

        // Assert
        Assert.Equal(6, _stack.Top);
        Assert.Equal(array.Length, _stack.Count);
    }

    [Fact]
    public void WhenDoPop_ThenGetTheTopElement()
    {
        // Arrange
        _stack.Push(6);

        // Act
        var top = _stack.Pop();
        _stack.Pop();
        _stack.Pop();

        Assert.Equal(6, top);
        Assert.Equal(3, _stack.Top);
    }
}