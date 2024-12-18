using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace ServicesTests;

public class UnbelievableClassTest
{
    [Fact]
    public void When_Then()
    {
        // Arrange
        var expect = true;

        // Act
        var actual = false;

        // Assert
        Assert.Skip("123");
        Assert.Equal(expect, actual);
    }
}