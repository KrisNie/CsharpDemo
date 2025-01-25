using Collections.DataStructure.List;
using Xunit.Abstractions;

namespace Collections.Tests.DataStructure;

public class DoublyLinkedListTest(ITestOutputHelper output)
{
    [Fact]
    public void WhenAppendOrRemoveItem_ThenDoAddOrRemoveItem()
    {
        // Arrange
        var list = new DoublyLinkedList<string>();
        list.Append("zero");
        list.Append("fst");
        list.Append("sec");
        list.Append("trd");
        list.Append("for");
        list.Append("fft");
        list.Append("sxt");
        list.Append("svn");
        list.Append("egt");
        // Act
        list.RemoveAt(0);
        var actualDataOfFirstNode = list[0];
        list.RemoveAt(4);
        var actualDataOfFourthNode = list[4];
        list.RemoveAt(5);
        list.RemoveAt(5);
        var actualListCount = list.Count;
        list.RemoveAt(list.Count - 1);
        var actualDataOfThirdNode = list[3];
        // Assert
        Assert.True(actualDataOfFirstNode == "fst", "Wrong first element.");
        Assert.True(actualDataOfFourthNode == "sxt", "Wrong 4th element.");
        Assert.True(actualListCount == 5);
        Assert.True(actualDataOfThirdNode == "for", "Wrong element at index 3.");
    }

    [Fact]
    public void WhenPrependOrInsertItem_ThenDoAddOrRemoveItem()
    {
        // Arrange
        var list = new DoublyLinkedList<string>();
        list.Append("zero");
        list.Append("fst");
        list.Append("sec");
        list.Append("trd");
        list.Append("for");
        list.Append("fft");
        list.Append("sxt");
        list.Append("svn");
        list.Append("egt");
        // Act
        list.RemoveAt(0);
        list.Prepend("semsem3");
        list.Prepend("semsem2");
        list.Prepend("semsem1");
        list.InsertAt("InsertedAtLast1", list.Count);
        list.InsertAt("InsertedAtLast2", list.Count);
        list.InsertAt("InsertedAtMiddle", (list.Count / 2));
        list.InsertAt("InsertedAt 4", 4);
        list.InsertAt("InsertedAt 9", 9);
        list.InsertAfter("InsertedAfter 11", 11);
        list.Remove("trd");
        list.Remove("InsertedAt 9");
        // Assert
        output.WriteLine(list.ToHumanReadable());
        Assert.Equal(
            "[0] => semsem1\r\n[1] => semsem2\r\n[2] => semsem3\r\n[3] => fst\r\n[4] => InsertedAt 4\r\n[5] => sec\r\n[6] => InsertedAtMiddle\r\n[7] => for\r\n[8] => InsertedAfter 11\r\n[9] => fft\r\n[10] => sxt\r\n[11] => svn\r\n[12] => egt\r\n[13] => InsertedAtLast1\r\n[14] => InsertedAtLast2\r\n",
            list.ToHumanReadable());
    }

    [Fact]
    public void WhenGetEnumerator_ThenReturnTheEnumerator()
    {
        // Arrange
        var list = new DoublyLinkedList<string>();
        list.Append("zero");
        list.Append("fst");
        list.Append("sec");
        // Act
        var enumerator = list.GetEnumerator();
        enumerator.MoveNext();
        var actualDataOfFirstNode = enumerator.Current;
        enumerator.MoveNext();
        var actualDataOfSecondNode = enumerator.Current;
        enumerator.MoveNext();
        var actualDataOfThirdNode = enumerator.Current;
        enumerator.Dispose();
        // Assert
        Assert.True(actualDataOfFirstNode == "zero", "Wrong enumeration.");
        Assert.True(actualDataOfSecondNode == "fst", "Wrong enumeration.");
        Assert.True(actualDataOfThirdNode == "sec", "Wrong enumeration.");
        Assert.True(list is { Count: > 0 }, "Enumerator has side effects!");
    }

    [Fact]
    public void WhenSelectionSort_ThenSortTheList()
    {
        // Arrange
        var list = new DoublyLinkedList<int>();
        list.Append(23);
        list.Append(42);
        list.Append(4);
        list.Append(16);
        list.Append(8);
        list.Append(15);
        list.Append(9);
        list.Append(55);
        list.Append(0);
        list.Append(34);
        list.Append(12);
        list.Append(2);
        // Act
        list.SelectionSort();
        // Assert
        output.WriteLine(list.ToHumanReadable());
        Assert.Equal(
            "[0] => 0\r\n[1] => 2\r\n[2] => 4\r\n[3] => 8\r\n[4] => 9\r\n[5] => 12\r\n[6] => 15\r\n[7] => 16\r\n[8] => 23\r\n[9] => 34\r\n[10] => 42\r\n[11] => 55\r\n",
            list.ToHumanReadable());
    }
}