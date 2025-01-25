using Collections.DataStructure.Heap;
using Xunit.Abstractions;

namespace Collections.Tests.DataStructure;

public class BinaryMinHeapTest(ITestOutputHelper output)
{
    [Fact]
    public void WhenAddItemInRandomOrder_ThenHeapIsStillTheMinHeap()
    {
        // Arrange
        var binaryMinHeap = new BinaryMinHeap<long>(Comparer<long>.Default);
        binaryMinHeap.Add(23);
        binaryMinHeap.Add(42);
        binaryMinHeap.Add(4);
        binaryMinHeap.Add(16);
        binaryMinHeap.Add(8);
        binaryMinHeap.Add(1);
        binaryMinHeap.Add(3);
        binaryMinHeap.Add(100);
        binaryMinHeap.Add(5);
        binaryMinHeap.Add(7);
        // Act
        var isRightOrder = IsRightOrderInHeap(binaryMinHeap);
        // Assert
        Assert.True(isRightOrder);
        Assert.Equal(
            "[0] => 1\r\n[1] => 5\r\n[2] => 3\r\n[3] => 8\r\n[4] => 7\r\n[5] => 23\r\n[6] => 4\r\n[7] => 100\r\n[8] => 42\r\n[9] => 16\r\n",
            binaryMinHeap.ToHumanReadable());
    }

    [Fact]
    public void WhenAddItemInAscendingOrder_ThenHeapIsStillTheMinHeap()
    {
        // Arrange
        var binaryMinHeap = new BinaryMinHeap<long>(Comparer<long>.Default);
        binaryMinHeap.Add(1);
        binaryMinHeap.Add(2);
        binaryMinHeap.Add(3);
        binaryMinHeap.Add(4);
        binaryMinHeap.Add(5);
        binaryMinHeap.Add(6);
        binaryMinHeap.Add(7);
        binaryMinHeap.Add(8);
        binaryMinHeap.Add(9);
        binaryMinHeap.Add(10);
        // Act
        var isRightOrder = IsRightOrderInHeap(binaryMinHeap);
        // Assert
        Assert.True(isRightOrder);
        Assert.Equal(
            "[0] => 1\r\n[1] => 2\r\n[2] => 3\r\n[3] => 4\r\n[4] => 5\r\n[5] => 6\r\n[6] => 7\r\n[7] => 8\r\n[8] => 9\r\n[9] => 10\r\n",
            binaryMinHeap.ToHumanReadable());
    }

    [Fact]
    public void WhenAddItemInDecreasingOrder_ThenHeapIsStillTheMinHeap()
    {
        // Arrange
        var binaryMinHeap = new BinaryMinHeap<long>(Comparer<long>.Default);
        binaryMinHeap.Add(10);
        binaryMinHeap.Add(9);
        binaryMinHeap.Add(8);
        binaryMinHeap.Add(7);
        binaryMinHeap.Add(6);
        binaryMinHeap.Add(5);
        binaryMinHeap.Add(4);
        binaryMinHeap.Add(3);
        binaryMinHeap.Add(2);
        binaryMinHeap.Add(1);
        // Assert
        Assert.True(IsRightOrderInHeap(binaryMinHeap));
        Assert.Equal(
            "[0] => 1\r\n[1] => 2\r\n[2] => 5\r\n[3] => 4\r\n[4] => 3\r\n[5] => 9\r\n[6] => 6\r\n[7] => 10\r\n[8] => 7\r\n[9] => 8\r\n",
            binaryMinHeap.ToHumanReadable());
    }

    private static bool IsRightOrderInHeap<T>(BinaryMinHeap<T> binaryMinHeap)
        where T : IComparable<T>
    {
        var array = binaryMinHeap.ToArray();
        for (var i = 0; i * 2 + 1 < array.Length; ++i)
        {
            var leftChildIndex = i * 2 + 1;
            var rightChildIndex = leftChildIndex + 1;
            if (array[i].CompareTo(array[leftChildIndex]) > 0) return false;
            if (rightChildIndex < array.Length && array[i].CompareTo(array[rightChildIndex]) > 0)
                return true;
        }

        return true;
    }
}