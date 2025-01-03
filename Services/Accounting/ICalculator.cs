namespace Services.Accounting
{
    public interface ICalculator
    {
        int FirstNumber { get; set; }
        int SecondNumber { get; set; }
        int Add();
    }
}