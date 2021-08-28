using Moq;
using NUnit.Framework;
using Services.Finance;

namespace ServicesTests
{
    public class BankAccountTests
    {
        private Mock<ICalculator> _mockCalculator;

        private IBankAccount _mockBankAccountObject;
        private const double BeginningBalance = 11.99;


        [SetUp]
        public void Setup()
        {
            _mockCalculator = new Mock<ICalculator>();

            _mockBankAccountObject = new BankAccount(_mockCalculator.Object);
        }

        [Test]
        public void WhenSuccess_ThenSuccessMessage()
        {
            // Arrange
            const double debitAmount = 4.55;
            const double expected = 7.44;
            _mockBankAccountObject.Create("Mr. Bryan Walton", BeginningBalance);

            // Act
            _mockBankAccountObject.Debit(debitAmount); // 11.99 - 4.55 = 7.44

            // Assert
            Assert.AreEqual(expected, _mockBankAccountObject.Balance, 0.001, "Account not debited correctly");
        }
    }
}