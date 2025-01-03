using System;

namespace Services.Accounting;

public class BankAccount(ICalculator calculator) : IBankAccount
{
    private ICalculator _calculator = calculator;
    public string Age { get; private set; }
    public string CustomerName { get; private set; }
    public double Balance { get; private set; }

    public void Create(string customerName, double balance)
    {
        CustomerName = customerName;
        Balance = balance;
    }

    public void Debit(double amount)
    {
        if (amount > Balance) throw new ArgumentOutOfRangeException(nameof(amount));

        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        // intentionally incorrect code
        // Balance += amount;
        Balance -= amount;
    }

    public void Credit(double amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

        Balance += amount;
    }
}