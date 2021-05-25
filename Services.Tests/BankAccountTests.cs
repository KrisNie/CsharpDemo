using Moq;
using NUnit.Framework;
using Services;

namespace ServicesTests
{
    public class BankAccountTests
    {
        private Mock<UnbelievableClass> _mockUnbelievableClass;

        [SetUp]
        public void Setup()
        {
            _mockUnbelievableClass = new Mock<UnbelievableClass>();
        }

        [Test]
        public void WhenSuccess_ThenSuccessMessage()
        {
            // Arrange
            const double beginningBalance = 11.99;
            const double debitAmount = 4.55;
            const double expected = 7.44;
            var account = new BankAccount("Mr. Bryan Walton", beginningBalance);

            // Act
            account.Debit(debitAmount); // 11.99 - 4.55 = 7.44

            // Assert
            Assert.AreEqual(expected, account.Balance, 0.001, "Account not debited correctly");
        }
    }
}