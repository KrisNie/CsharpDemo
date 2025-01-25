using Collections.DataStructure.List;

namespace Collections.Tests.DataStructure;

public class ArrayListTest
{
    [Fact]
    public void WhenFind_ThenReturnTheTargetElement()
    {
        // Arrange
        // Act
        var arrayList = new ArrayList<long>();
        for (long i = 1; i < 1000000; ++i) arrayList.Add(i);
        for (var i = 1000; i < 1100; i++) arrayList.RemoveAt(i);
        for (var i = 100000; i < 100100; i++) arrayList.Remove(i);
        var allNumbersBiggerThanNineHundK = arrayList.FindAll(item => item > 900000);
        var resultOfFind = arrayList.Find(item => item == 900000);
        var indexOfResultOfFind = arrayList.FindIndex(item => item == resultOfFind);
        const int index = 900000;
        arrayList.InsertAt(99999, index);
        arrayList.InsertAt(99999, index);
        arrayList.InsertAt(99999, index);
        arrayList.InsertAt(99999, index);
        arrayList.InsertAt(99999, index);
        var resultOfFindAll = arrayList.FindAll(item => item == 99999);
        var resultOfExists = arrayList.Exists(item => item == 1000000);
        var resultOfContains = arrayList.Contains(88888);
        // Assert
        Assert.Multiple(
            () =>
            {
                Assert.True(allNumbersBiggerThanNineHundK.Count > 0, "Count check failed!");
                Assert.True(indexOfResultOfFind != -1, "Wrong index!");
                Assert.True(resultOfFindAll.Count == 6, "Wrong result!");
                Assert.False(resultOfExists, "Wrong result!");
                Assert.True(resultOfContains, "Wrong result!");
            });
    }

    [Fact]
    public void WhenSetValueByIndex_ThenItemChanged()
    {
        // Arrange
        var arrayList = new ArrayList<int> { 1, 2, 3 };
        // Act
        arrayList[0] = 4;
        arrayList[1] = 5;
        arrayList[2] = 6;
        // Assert
        Assert.Equal("[0] => 4\r\n[1] => 5\r\n[2] => 6\r\n", arrayList.ToHumanReadable());
    }

    // TODO: Make coverage better.
}