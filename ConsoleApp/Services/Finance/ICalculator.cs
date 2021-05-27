namespace Services.Finance
{
    public interface ICalculator
    {
        int FirstNumber { get; set; }
        int SecondNumber { get; set; }

        int Add();
    }
}