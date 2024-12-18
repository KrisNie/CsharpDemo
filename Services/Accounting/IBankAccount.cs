namespace Services.Accounting
{
    public interface IBankAccount
    {
        string CustomerName { get; }
        double Balance { get; }

        void Create(string customerName, double balance);
        void Debit(double amount);
        void Credit(double amount);
    }
}