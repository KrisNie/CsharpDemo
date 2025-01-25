using Collections.DataStructure.Heap;
using Xunit.Abstractions;

namespace Collections.Tests.DataStructure;

public class BinaryMaxHeapTest(ITestOutputHelper output)
{
    [Fact]
    public void WhenAddItemInRandomOrder_ThenHeapIsStillTheMaxHeap()
    {
        // Arrange
        var binaryMaxHeap = new BinaryMaxHeap<long>(Comparer<long>.Default);
        binaryMaxHeap.Add(23);
        binaryMaxHeap.Add(42);
        binaryMaxHeap.Add(4);
        binaryMaxHeap.Add(16);
        binaryMaxHeap.Add(8);
        binaryMaxHeap.Add(1);
        binaryMaxHeap.Add(3);
        binaryMaxHeap.Add(100);
        binaryMaxHeap.Add(5);
        binaryMaxHeap.Add(7);
        // Act
        var isRightOrder = IsRightOrderInHeap(binaryMaxHeap);
        // Assert
        Assert.True(isRightOrder);
        Assert.Equal(
            "[0] => 100\r\n[1] => 42\r\n[2] => 4\r\n[3] => 23\r\n[4] => 8\r\n[5] => 1\r\n[6] => 3\r\n[7] => 16\r\n[8] => 5\r\n[9] => 7\r\n",
            binaryMaxHeap.ToHumanReadable());
    }

    [Fact]
    public void WhenAddItemInAscendingOrder_ThenHeapIsStillTheMaxHeap()
    {
        // Arrange
        var binaryMaxHeap = new BinaryMaxHeap<long>(Comparer<long>.Default);
        binaryMaxHeap.Add(1);
        binaryMaxHeap.Add(2);
        binaryMaxHeap.Add(3);
        binaryMaxHeap.Add(4);
        binaryMaxHeap.Add(5);
        binaryMaxHeap.Add(6);
        binaryMaxHeap.Add(7);
        binaryMaxHeap.Add(8);
        binaryMaxHeap.Add(9);
        binaryMaxHeap.Add(10);
        // Act
        var isRightOrder = IsRightOrderInHeap(binaryMaxHeap);
        // Assert
        Assert.True(isRightOrder);
        Assert.Equal(
            "[0] => 10\r\n[1] => 9\r\n[2] => 6\r\n[3] => 7\r\n[4] => 8\r\n[5] => 2\r\n[6] => 5\r\n[7] => 1\r\n[8] => 4\r\n[9] => 3\r\n",
            binaryMaxHeap.ToHumanReadable());
    }

    [Fact]
    public void WhenAddItemInDecreasingOrder_ThenHeapIsStillTheMaxHeap()
    {
        // Arrange
        var binaryMaxHeap = new BinaryMaxHeap<long>(Comparer<long>.Default);
        binaryMaxHeap.Add(10);
        binaryMaxHeap.Add(9);
        binaryMaxHeap.Add(8);
        binaryMaxHeap.Add(7);
        binaryMaxHeap.Add(6);
        binaryMaxHeap.Add(5);
        binaryMaxHeap.Add(4);
        binaryMaxHeap.Add(3);
        binaryMaxHeap.Add(2);
        binaryMaxHeap.Add(1);
        // Act
        var isRightOrder = IsRightOrderInHeap(binaryMaxHeap);
        // Assert        
        Assert.True(isRightOrder);
        Assert.Equal(
            "[0] => 10\r\n[1] => 9\r\n[2] => 8\r\n[3] => 7\r\n[4] => 6\r\n[5] => 5\r\n[6] => 4\r\n[7] => 3\r\n[8] => 2\r\n[9] => 1\r\n",
            binaryMaxHeap.ToHumanReadable());
    }

    private static bool IsRightOrderInHeap<T>(BinaryMaxHeap<T> binaryMaxHeap)
        where T : IComparable<T>
    {
        var array = binaryMaxHeap.ToArray();
        for (var i = 0; i * 2 + 1 < array.Length; ++i)
        {
            var leftChildIndex = i * 2 + 1;
            var rightChildIndex = leftChildIndex + 1;
            if (array[i].CompareTo(array[leftChildIndex]) < 0) return false;
            if (rightChildIndex < array.Length && array[i].CompareTo(array[rightChildIndex]) > 0)
                return true;
        }

        return true;
    }
}