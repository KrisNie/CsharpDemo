using Collections.DataStructure.Lists;

namespace Collection.Tests.DataStructure;

public class CircularBufferTest
{
    [Fact]
    public void SetsFixedLength()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(3);
        // Act
        var length = circularBuffer.Capacity;
        // Assert
        Assert.Equal(3, length);
    }

    [Fact]
    public void ChecksIsEmptyProperty()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(4);
        // Act
        // Assert
        Assert.True(circularBuffer.IsEmpty);
    }

    [Fact]
    public void ChecksIsFilledProperty()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(3, false);
        // Act
        circularBuffer.Add(1);
        circularBuffer.Add(2);
        circularBuffer.Add(3);
        // Assert
        Assert.True(circularBuffer.IsFull);
    }

    [Fact]
    public void InitializesWithDefaultLengthOf10()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>();
        // Act
        var length = circularBuffer.Capacity;
        // Assert
        Assert.Equal(10, length);
    }

    [Fact]
    public void ThrowsArgumentOutOfRangeExceptionForLengthLessThanOne()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () =>
            {
                var _ = new CircularBuffer<byte>(0);
            });
        Assert.Throws<ArgumentOutOfRangeException>(
            () =>
            {
                var _ = new CircularBuffer<byte>(-2);
            });
    }

    [Fact]
    public void ThrowsCircularBufferFullExceptionWhenInsertingInFullBuffer()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<CircularBufferFullException>(
            () =>
            {
                var _ = new CircularBuffer<byte>(3, false)
                {
                    1,
                    2,
                    3,
                    4
                };
            });
    }

    [Fact]
    public void WritesAndReadsValue()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(4)
        {
            13,
            43,
            23,
            2
        };
        // Act
        var result1 = circularBuffer.Pop();
        var result2 = circularBuffer.Pop();
        var result3 = circularBuffer.Pop();
        var result4 = circularBuffer.Pop();
        var result5 = circularBuffer.Pop();
        var result6 = circularBuffer.Pop();
        var result7 = circularBuffer.Pop();
        var result8 = circularBuffer.Pop();
        // Assert
        Assert.Equal(13, result1);
        Assert.Equal(43, result2);
        Assert.Equal(23, result3);
        Assert.Equal(2, result4);
        Assert.Equal(0, result5);
        Assert.Equal(0, result6);
        Assert.Equal(0, result7);
        Assert.Equal(0, result8);
    }

    [Fact]
    public void TestingCantOverrideFunctionality()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(3, false)
        {
            3,
            34,
            24
        };

        // Act
        // Assert
        Assert.Throws<CircularBufferFullException>(() => { circularBuffer.Add(2); });
        Assert.Equal(3, circularBuffer.Pop());
        Assert.Equal(34, circularBuffer.Pop());
        Assert.Equal(24, circularBuffer.Pop());
    }

    [Fact]
    public void TestingWritingAndReadingSimultaneouslyWithoutOverriding()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(3, false)
        {
            3,
            34,
            24
        };
        // Act
        circularBuffer.Pop();
        circularBuffer.Pop();
        circularBuffer.Add(4);
        circularBuffer.Add(14);
        // Assert
        Assert.Equal(24, circularBuffer.Pop());
        Assert.Equal(4, circularBuffer.Pop());
        Assert.Equal(14, circularBuffer.Pop());
    }

    [Fact]
    public void TestingICollectionImplementation()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(3, false)
        {
            3,
            34,
            24
        };
        var array = new byte[3];
        // Act
        circularBuffer.CopyTo(array, 0);
        circularBuffer.Clear();
        // Assert
        Assert.Equal(3, array[0]);
        Assert.Equal(34, array[1]);
        Assert.Equal(24, array[2]);
        Assert.Equal(0, circularBuffer.Pop());
        Assert.Equal(0, circularBuffer.Pop());
        Assert.Equal(0, circularBuffer.Pop());
        Assert.Empty(circularBuffer);
    }

    [Fact]
    public void WhenRemoveItemNotExists_ThenRemoveNothing()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(5, false);
        circularBuffer.Add(3);
        circularBuffer.Add(34);
        circularBuffer.Add(24);
        circularBuffer.Add(31);
        circularBuffer.Add(14);
        // Act
        circularBuffer.Remove(0);
        // Assert
        Assert.Equal(3, circularBuffer.Pop());
        Assert.Equal(34, circularBuffer.Pop());
        Assert.Equal(24, circularBuffer.Pop());
        Assert.Equal(31, circularBuffer.Pop());
        Assert.Equal(14, circularBuffer.Pop());
    }

    [Fact]
    public void WhenRemoveNumbers_ThenRemoveAllTargetNumbers()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(5, false)
        {
            3,
            3,
            3,
            31,
            14
        };
        // Act
        circularBuffer.Remove(3);
        // Arrange
        Assert.Equal(5 - 3, circularBuffer.Count);
    }

    [Fact]
    public void WhenRemoveAllItems_ThenCircularBufferIsEmpty()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(3, false)
        {
            1,
            2,
            3
        };
        // Act
        circularBuffer.Remove(3);
        circularBuffer.Remove(2);
        circularBuffer.Remove(1);
        // Assert
        Assert.Empty(circularBuffer);
    }

    [Fact]
    public void WhenRemoveNumbers_ThenRemoveAllTargetNumbersAndPopSequentially()
    {
        // Arrange
        var circularBuffer = new CircularBuffer<byte>(12, false)
        {
            1,
            0,
            2,
            0,
            3,
            0,
            4,
            0,
            5
        };
        // Act
        circularBuffer.Remove(0);
        // Assert
        Assert.Equal(5, circularBuffer.Count);
        Assert.Equal(1, circularBuffer.Pop());
        Assert.Equal(2, circularBuffer.Pop());
        Assert.Equal(3, circularBuffer.Pop());
        Assert.Equal(4, circularBuffer.Pop());
        Assert.Equal(5, circularBuffer.Pop());
    }

    [Fact]
    public void WhenRemoveDuplicateStringsValues_ThenRemoveAllTargetItem()
    {
        // Arrange
        var stringBuffer = new CircularBuffer<string?>(10, false)
        {
            "one",
            null,
            "two",
            null,
            "three",
            null,
            "four",
            null,
            "five"
        };
        // Act
        stringBuffer.Remove(null);
        // Assert        
        Assert.Equal(5, stringBuffer.Count);
        Assert.Equal("one", stringBuffer.Pop());
        Assert.Equal("two", stringBuffer.Pop());
        Assert.Equal("three", stringBuffer.Pop());
        Assert.Equal("four", stringBuffer.Pop());
        Assert.Equal("five", stringBuffer.Pop());
    }
}