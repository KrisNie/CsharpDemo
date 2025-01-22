using Collections.DataStructure.Lists;
using Xunit.Abstractions;

namespace Collections.Tests.DataStructure;

public class SinglyLinkedListTest(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public void WhenAppendAndRemove_ThenAddOrDeleteElement()
    {
        // Arrange
        var singlyLinkedList = new SinglyLinkedList<int>();
        // Act
        singlyLinkedList.Append(10);
        singlyLinkedList.Append(124);
        singlyLinkedList.Prepend(654);
        singlyLinkedList.Prepend(8);
        singlyLinkedList.Append(127485693);
        singlyLinkedList.Append(34);
        singlyLinkedList.Append(823);
        singlyLinkedList.RemoveAt(0);
        singlyLinkedList.RemoveAt(3);
        singlyLinkedList.RemoveAt(4);
        singlyLinkedList.RemoveAt(2);
        singlyLinkedList.RemoveAt(2);
        singlyLinkedList.RemoveAt(0);
        singlyLinkedList.Prepend(3);
        singlyLinkedList.Prepend(2);
        singlyLinkedList.Prepend(1);
        singlyLinkedList[singlyLinkedList.Count] = 444;
        singlyLinkedList[singlyLinkedList.Count] = 555;
        singlyLinkedList[2] = 222;
        var arrayVersion = singlyLinkedList.ToArray();
        // Assert
        Assert.Multiple(
            () =>
            {
                Assert.Equal(
                    "[0] => 1\r\n[1] => 2\r\n[2] => 222\r\n[3] => 3\r\n[4] => 10\r\n[5] => 444\r\n[6] => 555\r\n",
                    singlyLinkedList.ToHumanReadable());
                Assert.True(arrayVersion.Length == singlyLinkedList.Count);
            });
    }

    [Fact]
    public void WhenSelectionSort_ThenSort()
    {
        // Arrange
        var singlyLinkedList = new SinglyLinkedList<int>();
        singlyLinkedList.Append(23);
        singlyLinkedList.Append(42);
        singlyLinkedList.Append(4);
        singlyLinkedList.Append(16);
        singlyLinkedList.Append(8);
        singlyLinkedList.Append(15);
        singlyLinkedList.Append(9);
        singlyLinkedList.Append(55);
        singlyLinkedList.Append(0);
        singlyLinkedList.Append(34);
        singlyLinkedList.Append(12);
        singlyLinkedList.Append(2);
        // Act
        singlyLinkedList.SelectionSort();
        // Assert
        Assert.Equal(
            "[0] => 0\r\n[1] => 2\r\n[2] => 4\r\n[3] => 8\r\n[4] => 9\r\n[5] => 12\r\n[6] => 15\r\n[7] => 16\r\n[8] => 23\r\n[9] => 34\r\n[10] => 42\r\n[11] => 55\r\n",
            singlyLinkedList.ToHumanReadable());
    }
}